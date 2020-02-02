/*
 * Text Cleaner - A utility to cleanup text
 * Copyright (C) 2020 Peter Stuifzand <peter@p83.nl>
 *
 * This file is part of Text Cleaner.
 *
 * Text Cleaner is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Text Cleaner is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Text Cleaner.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;

namespace TextCleaner
{
    public class OperationFactory
    {
        private readonly Dictionary<string, Type> operations = new Dictionary<string, Type>();

        public OperationFactory(IEnumerable<Operation> operations)
        {
            foreach (var operation in operations) this.operations.Add(operation.Name, operation.GetType());
        }

        private Operation Create(string name)
        {
            if (operations.ContainsKey(name)) {
                var type = operations[name];
                return (Operation) Activator.CreateInstance(type);
            }

            return null;
        }

        public Operation Create(string name, string[] args)
        {
            var op = Create(name);
            return op == null ? null : new SavedOperation(op, args);
        }
    }
}
