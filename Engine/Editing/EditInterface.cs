using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    /// <summary>
    /// This interface provides a view of an object that can be edited.
    /// </summary>
    public interface EditInterface
    {
        /// <summary>
        /// Get a name for this interface.
        /// </summary>
        /// <returns>A String with the name of the interface.</returns>
        String getName();

        /// <summary>
        /// Determine if this EditInterface has any EditableProperties.
        /// </summary>
        /// <returns>True if the interface has some EditableProperties.</returns>
        bool hasEditableProperties();

        /// <summary>
        /// This function will return all properties of an EditInterface.
        /// </summary>
        /// <returns>A enumerable over all properties in the EditInterface or null if there aren't any.</returns>
        IEnumerable<EditableProperty> getEditableProperties();

        /// <summary>
        /// Return the EditablePropertyInfo for this interface that determines
        /// the layout of a single property. This can be null if
        /// hasEditableProperties is false.
        /// </summary>
        /// <returns>The EditablePropertyInfo for this interface.</returns>
        EditablePropertyInfo getPropertyInfo();

        /// <summary>
        /// Determine if this EditInterface has any SubEditInterfaces.
        /// </summary>
        /// <returns>True if the interface has some SubEditInterfaces.</returns>
        bool hasSubEditInterfaces();

        /// <summary>
        /// Get any SubEditInterfaces in this interface.
        /// </summary>
        /// <returns>An enumerable over all EditInterfaces that are part of this EditInterface or null if there aren't any.</returns>
        IEnumerable<EditInterface> getSubEditInterfaces();

        /// <summary>
        /// Determine if this EditInterface has any CreateSubObjectCommands.
        /// </summary>
        /// <returns>True if there are create commands.</returns>
        bool hasCreateSubObjectCommands();

        /// <summary>
        /// Get a list of commands for creating sub objects.
        /// </summary>
        /// <returns>An IEnumerable over all creation commands or null if there aren't any.</returns>
        IEnumerable<CreateEditInterfaceCommand> getCreateSubObjectCommands();

        /// <summary>
        /// Determine if this interface has a command to destroy itself.
        /// </summary>
        /// <returns>True if there is a destroy command.</returns>
        bool hasDestroyObjectCommand();

        /// <summary>
        /// Get a command that will destroy this object. This command must
        /// accept a single argument that is a EditUICallback. This is optional
        /// and can be null.
        /// </summary>
        /// <returns>A command that will destroy this EditInterface object or null if it cannot be destroyed.</returns>
        DestroyEditInterfaceCommand getDestroyObjectCommand();

        /// <summary>
        /// Determine if this interface can create and destroy properties. If
        /// this returns true both getCreatePropertyCommand and
        /// getDestroyPropertyCommand must be implemented.
        /// </summary>
        /// <returns>True if this interface can create and destroy properties.</returns>
        bool canAddRemoveProperties();

        /// <summary>
        /// Get the command that creates new properties.
        /// </summary>
        /// <returns>A CreateEditablePropertyCommand to create properties or null if it does not have one.</returns>
        CreateEditablePropertyCommand getCreatePropertyCommand();

        /// <summary>
        /// Get the command that destroys properties.
        /// </summary>
        /// <returns>A DestroyEditablePropertyCommand to destroy properties or null if it does not have one.</returns>
        DestroyEditablePropertyCommand getDestroyPropertyCommand();

        /// <summary>
        /// Get the object that will be sent as the target to the create and
        /// destroy commands.
        /// </summary>
        /// <returns>The object that will be sent as the target to the create and destroy commands.</returns>
        Object getCommandTargetObject();
    }
}
