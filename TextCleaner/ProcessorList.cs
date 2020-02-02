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
    internal class Command
    {
        public Command(Processor processor, Operation op)
        {
            Processor = processor;
            Op = op;
        }

        public Processor Processor { get; }
        public Operation Op { get; }
    }

    public struct CommandData
    {
        public static CommandData CreateInstance(string processor, string operation, int operationId, string arg1,
            string arg2)
        {
            return new CommandData(processor, operation, operationId, arg1, arg2);
        }

        public string Processor { get; }
        public string Operation { get; }
        public int OperationId { get; }
        public string Arg1 { get; }
        public string Arg2 { get; }

        private CommandData(string processor, string operation, int operationId, string arg1, string arg2)
        {
            Processor = processor;
            Operation = operation;
            OperationId = operationId;
            Arg1 = arg1;
            Arg2 = arg2;
        }

        public string OperationText()
        {
            var text = String.Format("{0}\n\tArg1: {1}\n\tArg2: {2}", Operation, Arg1, Arg2);

            if (Processor == "line") {
                text = "On each line:\n" + text;
            } else if (Processor == "full") {
                text = "On full text:\n" + text;
            }

            return text;
        }
    }

    public class ProcessorList : Processor
    {
        private readonly List<Command> commands = new List<Command>();

        public ProcessorList(OperationFactory operationFactory, IEnumerable<CommandData> commandData)
        {
            foreach (var data in commandData) {
                Processor processor;

                if (data.Processor == "full") {
                    processor = new FullTextProcessor();
                } else if (data.Processor == "line") {
                    processor = new LineProcessor();
                } else {
                    continue;
                }

                var operation = operationFactory.Create(data.Operation, new[] {data.Arg1, data.Arg2});

                commands.Add(new Command(processor,
                    new SavedOperation(operation, new[] {data.Arg1, data.Arg2})));
            }
        }

        public OperationResult Process(string input, Operation op, string[] args)
        {
            var output = input;

            foreach (var command in commands) {
                // TODO: Look how the highlighting can be supported.
                var result = command.Processor.Process(output, command.Op, args);
                if (!result.Keep) break;

                output = result.Text;
            }

            return new OperationResult(output);
        }
    }
}
