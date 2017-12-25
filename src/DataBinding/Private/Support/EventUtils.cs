// Copyright 2017 Andy Kernahan
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.ComponentModel;

namespace AK.DataBinding.Private.Support
{
    internal static class EventUtils
    {
        public static void Move(PropertyChangedEventHandler handler, object oldSender, object newSender)
        {
            Remove(handler, oldSender);
            Add(handler, newSender);
        }

        private static void Remove(PropertyChangedEventHandler handler, object sender)
        {
            if (sender is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged -= handler;
            }
        }

        private static void Add(PropertyChangedEventHandler handler, object sender)
        {
            if (sender is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += handler;
            }
        }
    }
}
