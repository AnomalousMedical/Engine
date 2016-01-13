using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Logging;
using System.Xml;
using System.IO;

namespace Engine
{
    public class PositionCollection
    {
        private const String POSITIONS_ELEMENT = "Positions";
        private const String POSITION_ELEMENT = "Position";
        private const String TRANSLATION_ELEMENT = "Translation";
        private const String ROTATION_ELEMENT = "Rotation";
        private const String NAME_ATTRIBUTE = "name";

        private static readonly char[] SEPS = { ',' };
        private const float DEG_TO_RAD = 0.0174532925f;

        private Dictionary<String, Position> positionDictionary = new Dictionary<string, Position>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="templateController">The TemplateController to grab templates from.</param>
        public PositionCollection()
        {
            
        }

        /// <summary>
        /// Load positions from a stream.
        /// </summary>
        /// <param name="stream"></param>
        public PositionCollection(Stream stream)
        {
            try
            {
                using (XmlTextReader textReader = new XmlTextReader(stream))
                {
                    this.loadPositions(textReader);
                }
            }
            catch (Exception e)
            {
                Log.Error("{0} error loading positions.\nReason: {2}", e.GetType().Name, e.Message);
            }
        }

        /// <summary>
        /// Load the SimObject instances from the given XML stream into the
        /// given SimObjectManagerDefinition.
        /// </summary>
        /// <param name="xmlReader">The XML stream to read instances from.</param>
        /// <param name="objectManager">The SimObjectManagerDefinition to fill with the new instances.</param>
        public void loadPositions(XmlReader xmlReader)
        {
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name.Equals(POSITION_ELEMENT))
                    {
                        String name = xmlReader.GetAttribute(NAME_ATTRIBUTE);
                        if (!positionDictionary.ContainsKey(name))
                        {
                            Position position = new Position(name);
                            while (!(xmlReader.Name == POSITION_ELEMENT && xmlReader.NodeType == XmlNodeType.EndElement) && xmlReader.Read())
                            {
                                if (xmlReader.NodeType == XmlNodeType.Element)
                                {
                                    if (xmlReader.Name == ROTATION_ELEMENT)
                                    {
                                        position.Rotation = readRotation(xmlReader);
                                    }
                                    else if (xmlReader.Name == TRANSLATION_ELEMENT)
                                    {
                                        position.Translation = readTranslation(xmlReader);
                                    }
                                }
                            }
                            positionDictionary.Add(name, position);
                        }
                        else
                        {
                            Log.Warning("Duplicate position named {0} found. Duplicate ignored.", name);
                        }
                    }
                }
            }
        }

        public void clearPositions()
        {
            positionDictionary.Clear();
        }

        public Position getPosition(String name)
        {
            Position pos = null;
            positionDictionary.TryGetValue(name, out pos);
            return pos;
        }

        public IEnumerable<Position> Positions
        {
            get
            {
                return positionDictionary.Values;
            }
        }

        /// <summary>
        /// Read a rotation from the XML stream.
        /// </summary>
        /// <param name="xmlReader"></param>
        /// <returns></returns>
        private Quaternion readRotation(XmlReader xmlReader)
        {
            Quaternion rotation = new Quaternion();
            xmlReader.Read();
            String[] rots = xmlReader.Value.Split(SEPS);
            //Euler angles
            if (rots.Length == 3)
            {
                rotation.setEuler(NumberParser.ParseFloat(rots[0]) * DEG_TO_RAD, NumberParser.ParseFloat(rots[1]) * DEG_TO_RAD, NumberParser.ParseFloat(rots[2]) * DEG_TO_RAD);
            }
            else
            {
                Log.Default.sendMessage("Error loading rotation does not contain 3 numbers value: {0}.", LogLevel.Warning, "ShapeLoading", xmlReader.Value);
                rotation = Quaternion.Identity;
            }
            return rotation;
        }

        /// <summary>
        /// Read a translation from the XML stream.
        /// </summary>
        /// <param name="textReader"></param>
        /// <returns></returns>
        private Vector3 readTranslation(XmlReader xmlReader)
        {
            Vector3 translation = new Vector3();
            xmlReader.Read();
            String[] locs = xmlReader.Value.Split(SEPS);
            if (locs.Length == 3)
            {
                translation.x = NumberParser.ParseFloat(locs[0]);
                translation.y = NumberParser.ParseFloat(locs[1]);
                translation.z = NumberParser.ParseFloat(locs[2]);
            }
            else
            {
                Log.Default.sendMessage("Error loading translation does not contain 3 numbers value: {0}.", LogLevel.Error, "ShapeLoading", xmlReader.Value);
                translation = Vector3.Zero;
            }
            return translation;
        }
    }
}
