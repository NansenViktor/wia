﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallWebsite.Utility {
    public static class DependencySorter {
        public static IEnumerable<T> DependencySort<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies) {
            var sorted = new List<T>();
            var visited = new HashSet<T>();

            foreach (var item in source)
                Visit(item, visited, sorted, dependencies);

            return sorted;
        }

        private static void Visit<T>(T item, ISet<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies) {
            if (visited.Contains(item))
                return;

            visited.Add(item);

            foreach (var dep in dependencies(item))
                Visit(dep, visited, sorted, dependencies);

            sorted.Add(item);
        }
    }
}