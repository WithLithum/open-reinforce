// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace OpenReinforce.Engine.Data.Models.Response
{
    public static class ResponseTableReader
    {
        private static readonly HashSet<string> ValidBackupTypes =
            new HashSet<string>()
            {
                "LocalPatrol",
                "StatePatrol",
                "LocalSWAT",
                "NooseSWAT",
                "LocalAir",
                "NooseAir",
                "Ambulance",
                "Firetruck",
            };

        public static ResponseTable ReadTable(Stream stream)
        {
            using var reader = XmlReader.Create(stream);

            return ReadTable(reader);
        }

        public static ResponseTable ReadTable(XmlReader reader)
        {
            var document = new XmlDocument();
            document.Load(reader);

            return ReadTable(document);
        }

        private static ResponseTable ReadTable(XmlDocument document)
        {
            var respTable = new ResponseTable();
            var root = document.FirstChild;
            if (root == null)
            {
                return respTable;
            }

            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                var node = root.ChildNodes.Item(i);
                ReadTypeNode(node, respTable);
            }

            return respTable;
        }

        private static void ReadTypeNode(XmlNode node, ResponseTable respTable)
        {
            if (!ValidBackupTypes.Contains(node.Name))
            {
                // Somebody wrote something not supposed to be there. Ignore that ;)
                return;
            }

            var dict = new Dictionary<string, string>(node.ChildNodes.Count);

            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                var child = node.ChildNodes.Item(i);

                var agencyNode = child.SelectSingleNode("Agency");
                if (agencyNode == null
                    || string.IsNullOrWhiteSpace(agencyNode.InnerText))
                {
                    continue;
                }

                dict.Add(child.Name, agencyNode.InnerText);
            }

            AssignBackupType(node.Name, dict, respTable);
        }

        private static void AssignBackupType(string name,
            Dictionary<string, string> dict,
            ResponseTable respTable)
        {
            switch (name)
            {
                case "LocalPatrol":
                    respTable.LocalPatrol = dict;
                    break;
                case "StatePatrol":
                    respTable.StatePatrol = dict;
                    break;
                case "LocalSWAT":
                    respTable.LocalSWAT = dict;
                    break;
                case "NooseSWAT":
                    respTable.NooseSWAT = dict;
                    break;
                case "LocalAir":
                    respTable.LocalAir = dict;
                    break;
                case "NooseAir":
                    respTable.NooseAir = dict;
                    break;
                case "Ambulance":
                    respTable.Ambulance = dict;
                    break;
                case "Firetruck":
                    respTable.Firetruck = dict;
                    break;
            }
        }
    }
}
