using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using MyGUIPlugin;
using Engine.Platform;
using Engine;
using Logging;
using System.IO;

namespace Anomalous.GuiFramework
{
    public class GUIManager : IDisposable
    {
        public event Action<ConfigFile> SaveUIConfiguration;
        public event Action<ConfigFile> LoadUIConfiguration;
        public event Action Disposing;

        private static String INFO_SECTION = "__Info_Section_Reserved__";
        private static String INFO_VERSION = "Version";
        private static String INFO_UISCALE = "UIScale";

        private ScreenLayoutManager screenLayoutManager;

        private bool mainGuiShowing = true;
        private bool saveWindowsOnExit = true;
        private bool compactMode = false;

        //Dialogs
        private DialogManager dialogManager;

        //Events
        public event Action MainGUIShown;
        public event Action MainGUIHidden;

        public GUIManager()
        {

        }

        public void Dispose()
        {
            if (Disposing != null)
            {
                Disposing.Invoke();
            }

            IDisposableUtil.DisposeIfNotNull(dialogManager);
        }

        public void createGUI(MDILayoutManager mdiManager, LayoutChain layoutChain, OSWindow window)
        {
            screenLayoutManager = new ScreenLayoutManager(window);
            screenLayoutManager.ScreenSizeChanging += ScreenLayoutManager_ScreenSizeChanging;
            screenLayoutManager.ScreenSizeChanged += screenLayoutManager_ScreenSizeChanged;

            //Dialogs
            dialogManager = new DialogManager(mdiManager);

            screenLayoutManager.LayoutChain = layoutChain;
            layoutChain.CompactMode = compactMode;

            ScreenLayoutManager_ScreenSizeChanging(window.WindowWidth, window.WindowHeight);
        }

        public void addLinkToChain(LayoutChainLink link)
        {
            screenLayoutManager.LayoutChain.addLink(link, false);
        }

        public void removeLinkFromChain(LayoutChainLink link)
        {
            screenLayoutManager.LayoutChain.removeLink(link);
        }

        public void pushRootContainer(String name)
        {
            screenLayoutManager.LayoutChain.activateLinkAsRoot(name);
        }

        public void deactivateLink(String name)
        {
            screenLayoutManager.LayoutChain.deactivateLink(name);
        }

        public void windowChanged(OSWindow newWindow)
        {
            screenLayoutManager.changeOSWindow(newWindow);
        }

        public void changeElement(LayoutElementName elementName, LayoutContainer container, Action removedCallback)
        {
            if (container != null)
            {
                container.Visible = true;
                container.bringToFront();
            }
            screenLayoutManager.LayoutChain.addContainer(elementName, container, removedCallback);
        }

        public void closeElement(LayoutElementName elementName, LayoutContainer container)
        {
            screenLayoutManager.LayoutChain.removeContainer(elementName, container);
        }

        public void setMainInterfaceEnabled(bool enabled)
        {
            if (mainGuiShowing != enabled)
            {
                screenLayoutManager.LayoutChain.SuppressLayout = true;
                if (enabled)
                {
                    dialogManager.reopenMainGUIDialogs();
                    if (MainGUIShown != null)
                    {
                        MainGUIShown.Invoke();
                    }
                }
                else
                {
                    dialogManager.closeMainGUIDialogs();
                    if (MainGUIHidden != null)
                    {
                        MainGUIHidden.Invoke();
                    }
                }
                mainGuiShowing = enabled;
                screenLayoutManager.LayoutChain.SuppressLayout = false;
                screenLayoutManager.LayoutChain.layout();
            }
        }

        public void addManagedDialog(Dialog dialog)
        {
            dialogManager.addManagedDialog(dialog);
        }

        public void addManagedDialog(MDIDialog dialog)
        {
            dialogManager.addManagedDialog(dialog);
        }

        public void removeManagedDialog(MDIDialog dialog)
        {
            dialogManager.removeManagedDialog(dialog);
        }

        public void autoDisposeDialog(MDIDialog autoDisposeDialog)
        {
            dialogManager.autoDisposeDialog(autoDisposeDialog);
        }

        /// <summary>
        /// Delete the windows file and tell the manager to not write a new one on close.
        /// </summary>
        /// <returns>True if the file was deleted. False otherwise.</returns>
        public bool deleteWindowsFile(String file)
        {
            if (file != null)
            {
                try
                {
                    File.Delete(file);
                    saveWindowsOnExit = false;
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error("Could not delete windows file. Reason: {0}", ex.Message);
                }
            }
            return false;
        }

        public void loadSavedUI(ConfigFile configFile, Version skipIfLessThan)
        {
            ConfigSection infoSection = configFile.createOrRetrieveConfigSection(INFO_SECTION);
            String versionString = infoSection.getValue(INFO_VERSION, "0.0.0.0");
            float uiScale = infoSection.getValue(INFO_UISCALE, ScaleHelper.ScaleFactor);
            Version version;
            try
            {
                version = new Version(versionString);
            }
            catch (Exception)
            {
                version = new Version("0.0.0.0");
            }
            if (version > skipIfLessThan)
            {
                if (uiScale.EpsilonEquals(ScaleHelper.ScaleFactor, 1e-3f)) //Don't load dialog positions if the scales do not match
                {
                    dialogManager.loadDialogLayout(configFile);
                }
            }
            if (LoadUIConfiguration != null)
            {
                LoadUIConfiguration.Invoke(configFile);
            }
        }

        public void saveUI(ConfigFile configFile, Version version)
        {
            //Dialogs
            if (saveWindowsOnExit)
            {
                ConfigSection infoSection = configFile.createOrRetrieveConfigSection(INFO_SECTION);
                infoSection.setValue(INFO_VERSION, version.ToString());
                infoSection.setValue(INFO_UISCALE, ScaleHelper.ScaleFactor);
                if (SaveUIConfiguration != null)
                {
                    SaveUIConfiguration.Invoke(configFile);
                }
                dialogManager.saveDialogLayout(configFile);
            }
        }

        private void screenLayoutManager_ScreenSizeChanged(int width, int height)
        {
            dialogManager.windowResized();
        }

        private void ScreenLayoutManager_ScreenSizeChanging(int width, int height)
        {
            this.CompactMode = (float)width / height < .9f;
        }

        public event ScreenSizeChangedDelegate ScreenSizeChanged
        {
            add
            {
                screenLayoutManager.ScreenSizeChanged += value;
            }
            remove
            {
                screenLayoutManager.ScreenSizeChanged -= value;
            }
        }

        public IEnumerable<LayoutElementName> NamedLinks
        {
            get
            {
                return screenLayoutManager.LayoutChain.NamedLinks;
            }
        }

        public bool MainGuiShowing
        {
            get
            {
                return mainGuiShowing;
            }
        }

        public bool CompactMode
        {
            get
            {
                return compactMode;
            }
            set
            {
                compactMode = value;
                if (screenLayoutManager.LayoutChain != null)
                {
                    screenLayoutManager.LayoutChain.CompactMode = compactMode;
                }
            }
        }

        public void layout()
        {
            screenLayoutManager.LayoutChain.layout();
        }
    }
}
