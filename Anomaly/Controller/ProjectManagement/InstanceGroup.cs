using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;
using System.IO;
using Engine.Saving;
using System.Windows.Forms;
using Logging;
using System.Xml;
using Engine;

namespace Anomaly
{
    public class InstanceGroup : IDisposable
    {
        private static CopySaver copySaver = new CopySaver();

        private String name;
        private Dictionary<String, InstanceGroup> groups = new Dictionary<string, InstanceGroup>();
        private Dictionary<String, InstanceFileInterface> instanceFiles = new Dictionary<String, InstanceFileInterface>();
        private String path;
        private bool instancesDisplayed = false; //True if instances for this group are currently in the scene.
        private SimObjectController simObjectController;

        private List<InstanceGroup> deletedGroups = new List<InstanceGroup>();
        private List<InstanceFileInterface> deletedInstances = new List<InstanceFileInterface>();

        private InstanceGroup parent = null;

        public InstanceGroup(String name, String path)
        {
            this.name = name;
            this.path = path;
        }

        public void Dispose()
        {
            foreach (InstanceGroup group in groups.Values)
            {
                group.Dispose();
            }

            foreach (InstanceFileInterface fileInterface in instanceFiles.Values)
            {
                fileInterface.Dispose();
            }
        }

        public void addGroup(InstanceGroup group)
        {
            group.parent = this;
            groups.Add(group.name, group);
            if (instancesDisplayed)
            {
                group.showInstances(simObjectController);
            }
            if (editInterface != null)
            {
                addGroupSubInterface(group);
            }
        }

        public void removeGroup(InstanceGroup group)
        {
            group.parent = null;
            groups.Remove(group.name);
            group.Dispose();
            deletedGroups.Add(group);
            if (editInterface != null)
            {
                editInterface.removeSubInterface(group);
            }
        }

        /// <summary>
        /// Add a instance and save the defintion.
        /// </summary>
        /// <param name="simObject"></param>
        public void addInstanceFile(String instanceName)
        {
            InstanceFileInterface fileInterface = new InstanceFileInterface(instanceName, AnomalyIcons.Instance, InstanceWriter.Instance.getInstanceFileName(this, instanceName), this);
            if (instancesDisplayed)
            {
                fileInterface.createInstance(simObjectController);
            }
            instanceFiles.Add(instanceName, fileInterface);
            onInstanceFileAdded(fileInterface);
        }

        public void addInstance(Instance instance)
        {
            InstanceFileInterface fileInterface = new InstanceFileInterface(instance.Name, AnomalyIcons.Instance, InstanceWriter.Instance.getInstanceFileName(this, instance.Name), this, instance);
            if (instancesDisplayed)
            {
                fileInterface.createInstance(simObjectController);
            }
            instanceFiles.Add(instance.Name, fileInterface);
            onInstanceFileAdded(fileInterface);
        }

        public bool hasInstance(String name)
        {
            return instanceFiles.ContainsKey(name);
        }

        public void removeInstanceFile(String instanceName)
        {
            InstanceFileInterface fileInterface = instanceFiles[instanceName];
            fileInterface.Deleted = true;
            fileInterface.Dispose();
            instanceFiles.Remove(instanceName);
            deletedInstances.Add(fileInterface);
            onInstanceFileRemoved(fileInterface);
        }

        public void showInstances(SimObjectController simObjectController)
        {
            this.simObjectController = simObjectController;
            this.instancesDisplayed = true;

            foreach (InstanceFileInterface instanceFileInterface in instanceFiles.Values)
            {
                instanceFileInterface.createInstance(simObjectController);
            }

            foreach (InstanceGroup group in groups.Values)
            {
                group.showInstances(simObjectController);
            }
        }

        public void destroyInstances(SimObjectController simObjectController)
        {
            this.simObjectController = null;
            this.instancesDisplayed = false;

            foreach (InstanceFileInterface instanceFileInterface in instanceFiles.Values)
            {
                instanceFileInterface.destroyInstance(simObjectController);
            }

            foreach (InstanceGroup group in groups.Values)
            {
                group.destroyInstances(simObjectController);
            }
        }

        internal void buildInstances(ScenePackage scenePackage)
        {
            foreach (InstanceFileInterface instanceFileInterface in instanceFiles.Values)
            {
                instanceFileInterface.buildInstance(scenePackage.SimObjectManagerDefinition);
            }

            foreach (InstanceGroup group in groups.Values)
            {
                group.buildInstances(scenePackage);
            }
        }

        public void updatePositions(PositionCollection positions)
        {
            foreach (InstanceGroup group in groups.Values)
            {
                group.updatePositions(positions);
            }

            foreach (InstanceFileInterface instance in instanceFiles.Values)
            {
                instance.updatePosition(positions);
            }
        }

        public void save(bool forceSave)
        {
            InstanceWriter.Instance.addInstanceGroup(this);
            foreach (InstanceGroup group in deletedGroups)
            {
                InstanceWriter.Instance.removeInstanceGroup(group);
            }
            deletedGroups.Clear();

            foreach (InstanceFileInterface instanceFile in deletedInstances)
            {
                instanceFile.save(forceSave);
            }
            deletedInstances.Clear();

            foreach (InstanceFileInterface instanceFile in instanceFiles.Values)
            {
                instanceFile.save(forceSave);
            }

            foreach (InstanceGroup group in groups.Values)
            {
                group.save(forceSave);
            }
        }

        /// <summary>
        /// Search for the InstanceGroup that contains instanceName. Will return null if the instance does not exist.
        /// </summary>
        /// <param name="instanceName">The instance name to search for.</param>
        /// <returns>The group containing the instance or null if it does not exist.</returns>
        public InstanceGroup findInstanceGroup(String instanceName)
        {
            //recurse up to the parent.
            if (parent != null)
            {
                return parent.findInstanceGroup(instanceName);
            }
            else //if we are the top level search all nodes.
            {
                return doFindInstanceGroup(instanceName);
            }
        }

        /// <summary>
        /// The method that actually searches for the instance name. Called only by findInstanceGroup.
        /// </summary>
        /// <param name="instanceName">The instance name to search for.</param>
        /// <returns>The group containing the instance or null if it does not exist.</returns>
        private InstanceGroup doFindInstanceGroup(String instanceName)
        {
            if (instanceFiles.ContainsKey(instanceName))
            {
                return this;
            }
            else
            {
                foreach (InstanceGroup group in groups.Values)
                {
                    InstanceGroup foundGroup = group.doFindInstanceGroup(instanceName);
                    if (foundGroup != null)
                    {
                        return foundGroup;
                    }
                }
            }
            return null;
        }

        #region Properties

        public String Name
        {
            get
            {
                return name;
            }
        }

        public String FullPath
        {
            get
            {
                return path;
            }
        }

        #endregion Properties

        #region EditInterface

        private EditInterface editInterface;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(name);
                editInterface.addCommand(new EditInterfaceCommand("Create Group", createGroupCallback));
                editInterface.addCommand(new EditInterfaceCommand("Create Sim Object", createSimObjectCallback));
                editInterface.addCommand(new EditInterfaceCommand("Show All", showAllCallback));
                editInterface.addCommand(new EditInterfaceCommand("Hide All", hideAllCallback));
                editInterface.addCommand(new EditInterfaceCommand("Import Positions", importPositionsCallback));
                editInterface.IconReferenceTag = AnomalyIcons.Folder;
                GenericClipboardEntry clipboardEntry = new GenericClipboardEntry(typeof(InstanceGroup));
                clipboardEntry.PasteFunction = pasteCallback;
                clipboardEntry.SupportsPastingTypeFunction = supportsPasteType;
                editInterface.ClipboardEntry = clipboardEntry;

                var groupManager = editInterface.createEditInterfaceManager<InstanceGroup>();
                groupManager.addCommand(new EditInterfaceCommand("Remove", destroyGroupCallback));
                var instanceFileManager = editInterface.createEditInterfaceManager<InstanceFileInterface>();
                instanceFileManager.addCommand(new EditInterfaceCommand("Remove", destroySimObjectCallback));
                instanceFileManager.addCommand(new EditInterfaceCommand("Rename", renameSimObjectCallback));
                foreach (InstanceGroup group in groups.Values)
                {
                    addGroupSubInterface(group);
                }
                foreach (InstanceFileInterface file in instanceFiles.Values)
                {
                    onInstanceFileAdded(file);
                }
            }
            return editInterface;
        }

        private void addGroupSubInterface(InstanceGroup group)
        {
            editInterface.addSubInterface(group, group.getEditInterface());
        }

        public void pasteCallback(object pasted)
        {
            SimObjectDefinition simObject = (SimObjectDefinition)pasted;
            if (hasInstance(simObject.Name))
            {
                String namebase = simObject.Name + " - Copy";
                simObject.Name = namebase;
                int i = 1;
                while (hasInstance(simObject.Name))
                {
                    simObject.Name = namebase + i++;
                }
            }
            createInstance(simObject);
        }

        public bool supportsPasteType(Type type)
        {
            return typeof(SimObjectDefinition).IsAssignableFrom(type);
        }

        private void createGroupCallback(EditUICallback callback)
        {
            callback.getInputString("Enter a name.", delegate(String input, ref String errorPrompt)
            {
                if (input == null || input == "")
                {
                    errorPrompt = "Please enter a non empty name.";
                    return false;
                }
                if (this.groups.ContainsKey(input))
                {
                    errorPrompt = "That name is already in use. Please provide another.";
                    return false;
                }

                InstanceGroup group = new InstanceGroup(input, Path.Combine(path, input));
                this.addGroup(group);
                return true;
            });
        }

        private void destroyGroupCallback(EditUICallback callback)
        {
            removeGroup(editInterface.resolveSourceObject<InstanceGroup>(callback.getSelectedEditInterface()));
        }

        private void onInstanceFileAdded(InstanceFileInterface fileInterface)
        {
            if (editInterface != null)
            {
                editInterface.addSubInterface(fileInterface, fileInterface.getEditInterface());
            }
        }

        private void onInstanceFileRemoved(InstanceFileInterface fileInterface)
        {
            if (editInterface != null)
            {
                editInterface.removeSubInterface(fileInterface);
            }
        }

        private void createSimObjectCallback(EditUICallback callback)
        {
            callback.getInputString("Enter a name.", delegate(String input, ref String errorPrompt)
            {
                if (input == null || input == "")
                {
                    errorPrompt = "Please enter a non empty name.";
                    return false;
                }
                InstanceGroup groupWithInstance = findInstanceGroup(input);
                if (groupWithInstance != null)
                {
                    errorPrompt = String.Format("The name {0} is already in use in group {1}. Please provide another.", input, groupWithInstance.Name);
                    return false;
                }

                SimObjectDefinition simObject = new GenericSimObjectDefinition(input);
                createInstance(simObject);
                return true;
            });
        }

        private void destroySimObjectCallback(EditUICallback callback)
        {
            EditInterface selected = callback.getSelectedEditInterface();
            if (selected.hasEditableProperties())
            {
                InstanceFileInterface instanceFile = this.editInterface.resolveSourceObject<InstanceFileInterface>(selected);
                removeInstanceFile(instanceFile.Name);
            }
        }

        private void renameSimObjectCallback(EditUICallback callback)
        {
            callback.getInputString("Enter a name.", delegate(String input, ref String errorPrompt)
            {
                if (input == null || input == "")
                {
                    errorPrompt = "Please enter a non empty name.";
                    return false;
                }
                if (this.instanceFiles.ContainsKey(input))
                {
                    errorPrompt = "That name is already in use. Please provide another.";
                    return false;
                }
                
                InstanceFileInterface instanceFile = callback.getSelectedEditInterface().getEditableProperties().First() as InstanceFileInterface;
                SimObjectDefinition sourceObject = instanceFile.Instance.Definition;
                SimObjectDefinition simObject = copySaver.copyObject(sourceObject) as SimObjectDefinition;
                simObject.Name = input;
                createInstance(simObject);
                removeInstanceFile(instanceFile.Name);
                return true;
            });
        }

        private void createInstance(SimObjectDefinition simObject)
        {
            Instance instance = new Instance(simObject.Name, simObject);
            this.addInstance(instance);
            instanceFiles[instance.Name].Modified = true;
        }

        private void importPositionsCallback(EditUICallback callback)
        {
            callback.showOpenFileDialog("*.positions|*.positions", delegate(String file, ref String errorPrompt)
            {
                PositionCollection positions = new PositionCollection();
                try
                {
                    using (XmlTextReader textReader = new XmlTextReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read)))
                    {
                        positions.loadPositions(textReader);
                    }
                    this.updatePositions(positions);
                }
                catch (Exception e)
                {
                    Log.Error("Could not load positions file {0} because:\n{1}", file, e.Message);
                }
                return true;
            });
        }

        private void doImportTemplates(String directory)
        {
            foreach (String template in Directory.GetFiles(directory, "*.tpl", SearchOption.TopDirectoryOnly))
            {
                createInstance(InstanceWriter.Instance.loadTemplate(template));
            }
            foreach (String groupDir in Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(groupDir);
                if ((dirInfo.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    String name = dirInfo.Name;
                    if (!groups.ContainsKey(groupDir))
                    {
                        InstanceGroup group = new InstanceGroup(name, Path.Combine(path, name));
                        addGroup(group);
                    }
                    groups[name].doImportTemplates(groupDir);
                }
            }
        }

        private void showAllCallback(EditUICallback callback)
        {
            foreach (InstanceFileInterface instance in instanceFiles.Values)
            {
                instance.setVisible(true);
            }

            foreach (InstanceGroup group in groups.Values)
            {
                group.showAllCallback(callback);
            }
        }

        private void hideAllCallback(EditUICallback callback)
        {
            foreach (InstanceFileInterface instance in instanceFiles.Values)
            {
                instance.setVisible(false);
            }

            foreach (InstanceGroup group in groups.Values)
            {
                group.hideAllCallback(callback);
            }
        }

        #endregion
    }
}
