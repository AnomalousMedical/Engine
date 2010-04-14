using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;

namespace Anomaly
{
    partial class ProjectData
    {
        private ProjectReferenceManager projectReferences;

        public ProjectData()
        {
            CreateSceneFile = false;
            projectReferences = new ProjectReferenceManager();
        }

        public bool CreateSceneFile { get; set; }

        public String SceneFileName { get; set; }

        public ProjectReferenceManager ProjectReferences
        {
            get
            {
                return projectReferences;
            }
        }
    }

    partial class ProjectData : Saveable
    {
        private const string CREATE_SCENE = "CreateSceneFile";
        private const string SCENE_FILE = "SceneFileName";
        private const string PROJECT_REFERENCES = "ProjectReferences";

        protected ProjectData(LoadInfo info)
        {
            CreateSceneFile = info.GetBoolean(CREATE_SCENE);
            SceneFileName = info.GetString(SCENE_FILE);
            projectReferences = info.GetValue<ProjectReferenceManager>(PROJECT_REFERENCES);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(CREATE_SCENE, CreateSceneFile);
            info.AddValue(SCENE_FILE, SceneFileName);
            info.AddValue(PROJECT_REFERENCES, projectReferences);
        }
    }
}
