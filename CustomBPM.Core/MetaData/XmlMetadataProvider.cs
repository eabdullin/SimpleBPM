// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlMetadataProvider.cs" company="Ingenius Systems">
//   Copyright (c) Ingenius Systems
//   Create on 29.04.2015 17:27:20 by Albert Zakiev
// </copyright>
// <summary>
//   Defines the XmlMetadataProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using CustomBPM.Core.Configuration;
using CustomBPM.Core.Interfaces;

namespace CustomBPM.Core.MetaData
{
    public class XmlMetadataProvider : IMetadataProvider
    {
        #region Private fields

        private readonly Dictionary<string, Process> _processes;

        #endregion

        #region C-tor

        public XmlMetadataProvider()
        {
            _processes = new Dictionary<string, Process>();

            var section = ConfigurationManager.GetSection("bpm") as BpmConfigurationSection;
            if (section != null)
            {
                LoadProcessFiles(section.Processes.Location);
            }
        }

        #endregion

        #region Public methods

        public Process GetProcess(string code)
        {
            if (_processes.ContainsKey(code))
            {
                return _processes[code];
            }

            throw new Exception("Process with code not found");
        }

        #endregion

        #region Private methods

        private void LoadProcessFiles(string path)
        {
            var directory = path;

            if (!Path.IsPathRooted(directory))
            {
                directory = "";//HostingEnvironment.MapPath(directory);
            }

            if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
            {
                var files = Directory.GetFiles(directory, BpmConstants.ProcessFileExtension);

                foreach (var file in files)
                {
                    var process = LoadFromFile(file);
                    if (process != null)
                    {
                        _processes.Add(process.Code, process);
                    }
                }
            }
        }

        private Process LoadFromFile(string path)
        {
            Process result = null;

            try
            {
                var serializer = new XmlSerializer(typeof(Process));

                using (var reader = new StreamReader(path))
                {
                    result = (Process)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                // ignore
            }

            return result;
        }

        #endregion
    }
}