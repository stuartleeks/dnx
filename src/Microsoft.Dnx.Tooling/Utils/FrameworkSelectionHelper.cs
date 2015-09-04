// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Microsoft.Dnx.Runtime.Helpers;

namespace Microsoft.Dnx.Tooling.Utils
{
    public static class FrameworkSelectionHelper
    {
        public static IEnumerable<FrameworkName> SelectFrameworks(Runtime.Project project,
                                                                  IDictionary<FrameworkName, string> specifiedFrameworks,
                                                                  FrameworkName fallbackFramework,
                                                                  out string errorMessage)
        {
            var projectFrameworks = new HashSet<FrameworkName>(
                project.GetTargetFrameworks()
                       .Select(c => c.FrameworkName));

            IEnumerable<FrameworkName> frameworks = null;

            if (projectFrameworks.Count > 0)
            {
                // Specified target frameworks have to be a subset of the project frameworks
                if (!ValidateFrameworks(projectFrameworks, specifiedFrameworks, out errorMessage))
                {
                    return null;
                }

                frameworks = specifiedFrameworks.Count > 0 ? specifiedFrameworks.Keys : (IEnumerable<FrameworkName>)projectFrameworks;
            }
            else
            {
                frameworks = new[] { fallbackFramework };
            }

            errorMessage = string.Empty;
            return frameworks;
        }

        private static bool ValidateFrameworks(HashSet<FrameworkName> projectFrameworks,
                                               IDictionary<FrameworkName, string> specifiedFrameworks,
                                               out string errorMessage)
        {
            foreach (var framework in specifiedFrameworks)
            {
                if (!projectFrameworks.Contains(framework.Key))
                {
                    errorMessage = framework.Value + " is not specified in project.json";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}