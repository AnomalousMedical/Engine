using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RpgMath;
using SceneTest.Battle.Spells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Services
{
    interface IPersistenceWriter
    {
        Persistence Load();
        void Save();
    }

    class PersistenceWriter : IPersistenceWriter, IDisposable
    {
        private Persistence persistence;
        private readonly ILogger<PersistenceWriter> logger;
        private JsonSerializer serializer;

        public PersistenceWriter(ILogger<PersistenceWriter> logger)
        {
            this.logger = logger;
            serializer = new JsonSerializer()
            {
                Formatting = Formatting.Indented,
            };
        }

        public void Dispose()
        {
            Save();
        }

        public void Save()
        {
            if (persistence == null) { return; }

            var outFile = GetSaveFile();
            using var stream = new StreamWriter(File.Open(outFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None));
            serializer.Serialize(stream, persistence);
            logger.LogInformation($"Wrote save to '{outFile}'.");
        }

        public Persistence Load()
        {
            var outFile = GetSaveFile();

            if (!File.Exists(outFile))
            {
                logger.LogInformation($"Creating new save.");
                persistence = new Persistence();

                {
                    var arch = Archetype.CreateHero();
                    persistence.Party.Members.Add(new Persistence.CharacterData()
                    {
                        PlayerSprite = nameof(Assets.Original.FighterPlayerSprite),
                        CharacterSheet = new CharacterSheet()
                        {
                            Name = "Bob",
                            Archetype = arch,
                            Level = 1,
                            MainHand = new Equipment()
                            {
                                AttackPercent = 100,
                                Attack = 18
                            }
                        },
                        PrimaryHandAsset = nameof(Assets.Original.Greatsword01),
                        SecondaryHandAsset = nameof(Assets.Original.ShieldOfReflection)
                    });
                }

                {
                    var arch = Archetype.CreateSage();
                    persistence.Party.Members.Add(new Persistence.CharacterData()
                    {
                        PlayerSprite = nameof(Assets.Original.MagePlayerSprite),
                        CharacterSheet = new CharacterSheet()
                        {
                            Name = "Magic Joe",
                            Archetype = arch,
                            Level = 1,
                            MainHand = new Equipment()
                            {
                                AttackPercent = 35,
                                MagicAttackPercent = 100,
                                Attack = 9
                            }
                        },
                        PrimaryHandAsset = nameof(Assets.Original.Staff07),
                        Spells = new string[] { nameof(Fir), nameof(Fyre), nameof(Meltdown) }
                    });
                }

                {
                    var arch = Archetype.CreateTank();
                    persistence.Party.Members.Add(new Persistence.CharacterData()
                    {
                        PlayerSprite = nameof(Assets.Original.ThiefPlayerSprite),
                        CharacterSheet = new CharacterSheet()
                        {
                            Name = "Stabby McStabface",
                            Archetype = arch,
                            Level = 1,
                            MainHand = new Equipment()
                            {
                                AttackPercent = 100,
                                Attack = 18
                            }
                        },
                        PrimaryHandAsset = nameof(Assets.Original.DaggerNew),
                        SecondaryHandAsset = nameof(Assets.Original.DaggerNew)
                    });
                }

                {
                    var arch = Archetype.CreateGuardian();
                    persistence.Party.Members.Add(new Persistence.CharacterData()
                    {
                        PlayerSprite = nameof(Assets.Original.ClericPlayerSprite),
                        CharacterSheet = new CharacterSheet()
                        {
                            Name = "Wendy",
                            Archetype = arch,
                            Level = 1,
                            MainHand = new Equipment()
                            {
                                AttackPercent = 100,
                                Attack = 25
                            }
                        },
                        PrimaryHandAsset = nameof(Assets.Original.BattleAxe6),
                        Spells = new String[] { nameof(Cure) }
                    });
                }
            }
            else
            {
                logger.LogInformation($"Loading save from '{outFile}'.");
                using var stream = new JsonTextReader(new StreamReader(File.Open(outFile, FileMode.Open, FileAccess.Read, FileShare.Read)));
                persistence = serializer.Deserialize<Persistence>(stream);
            }

            return persistence;
        }

        private String GetSaveFile()
        {
            var outDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Anomalous Adventure");
            var outFile = Path.Combine(outDir, "save.json");

            Directory.CreateDirectory(outDir);

            return outFile;
        }
    }
}
