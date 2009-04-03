using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Logging;

namespace Engine
{
    /// <summary>
    /// This is a superclass for commands that the engine can run. It defines a
    /// set of names for the command.
    /// </summary>
    public abstract class EngineCommand
    {
        #region Fields

        private String name;
        private String prettyName;
        private String helpText;
        private List<Delegate> executeDelegates = new List<Delegate>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="signature">The signature of the command.</param>
        /// <param name="prettyName">The pretty name of the command.</param>
        /// <param name="helpText">Some text that describes what the command does.</param>
        public EngineCommand(String name, String prettyName, String helpText)
        {
            this.name = name;
            this.prettyName = prettyName;
            this.helpText = helpText;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Execute function. This will run the command.
        /// </summary>
        /// <param name="args">The args for the command.</param>
        public Object execute(params Object[] args)
        {
            List<Type> typeList = new List<Type>(args.Length);
            foreach (Object arg in args)
            {
                typeList.Add(arg.GetType());
            }
            Delegate chosen = null;
            foreach (Delegate fp in executeDelegates)
            {
                ParameterInfo[] paramInfo = fp.Method.GetParameters();
                if (paramInfo.Length == args.Length)
                {
                    bool matches = true;
                    for (int i = 0; i < paramInfo.Length; i++)
                    {
                        matches &= (paramInfo[i].ParameterType == typeList[i] || paramInfo[i].ParameterType.IsSubclassOf(typeList[i]));
                    }
                    if (matches)
                    {
                        chosen = fp;
                        break;
                    }
                }
            }
            if (chosen != null)
            {
                return chosen.DynamicInvoke(args);
            }
            else
            {
                Log.Default.sendMessage("Attempted to invoke command {0} with invalid args. Command not run.", LogLevel.Error, "Engine", name);
                return null;
            }
        }

        /// <summary>
        /// Add a delegate to this command that will be executed if the
        /// arguments given to exectue match the delegate. Warning, the first
        /// function with matching arguments will be used, so do not put in two
        /// methods with the same signature or it is undefined which one will be
        /// called.
        /// </summary>
        /// <param name="fp"></param>
        protected void addDelegate(Delegate fp)
        {
            executeDelegates.Add(fp);
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The name of the command. Should contain only letters and numbers no
        /// symbols or spaces.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// A pretty name for the command. This can contain any characters
        /// needed and will most likely show up on a UI so it should be human
        /// readable.
        /// </summary>
        public String PrettyName
        {
            get
            {
                return prettyName;
            }
        }

        /// <summary>
        /// A brief description of what the command does. No more than a couple
        /// of sentences.
        /// </summary>
        public String HelpText
        {
            get
            {
                return helpText;
            }
        }

        #endregion Properties
    }
}
