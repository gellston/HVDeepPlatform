using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionDeepTool.Service
{
    public class ApplicationConfigService
    {

        public ApplicationConfigService()
        {

        }



        public string CurrentApplicationPath
        {
            get => AppDomain.CurrentDomain.BaseDirectory;
        }

        public string CurrentApplicationSettingPath
        {
            get
            {
                var path = this.CurrentApplicationPath + "Config" + Path.DirectorySeparatorChar;
                Directory.CreateDirectory(path);
                return path;
            }
        }

        public string TempClassificationMergePath
        {
            get
            {
                var path = this.CurrentApplicationPath + "TempMergeClassification" + Path.DirectorySeparatorChar;
                Directory.CreateDirectory(path);
                return path;
            }
        }

    }
}
