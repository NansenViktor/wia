﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using InstallWebsite.Model;
using Microsoft.Web.Administration;

namespace InstallWebsite.Tasks {
    internal class WebserverTask : ITask {
        public IEnumerable<Type> DependsUpon() {
            return new List<Type> {typeof (HostsTask)};
        }

        public void Execute(WebsiteContext context) {
            return;

            using (ServerManager manager = new ServerManager()) {
                Site site = manager.Sites.Add(context.ProjectName, "http", context.ProjectUrl,
                                              Path.Combine(context.CurrentDirectory, context.WebProjectName));

                var pool = manager.ApplicationPools.Add(context.ProjectName);

                pool.Enable32BitAppOnWin64 = context.Enable32Bit;
                pool.ManagedPipelineMode = context.AppPoolMode == AppPoolMode.Integrated
                                               ? ManagedPipelineMode.Integrated
                                               : ManagedPipelineMode.Classic;
                pool.ManagedRuntimeVersion = context.FrameworkVersion == 4 ? "v4.0" : "v3.5"; // TODO: make this better

                site.ApplicationDefaults.ApplicationPoolName = context.ProjectName;

                manager.CommitChanges();
            }
        }
    }
}