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

using System;
using System.ComponentModel;
using AK.DataBinding.Private.Support;
using static AK.DataBinding.BindingResult;

namespace AK.DataBinding.Private
{
    internal sealed class InstanceMemberBinding<TObject, TResult> : Binding<TResult>
    {
        private readonly Binding<TObject> _object;
        private readonly string _name;
        private readonly Func<TObject, TResult> _accessor;
        private readonly BindingMode _mode;

        private static readonly bool TryObserveOnTObject;

        static InstanceMemberBinding()
        {
            var type = typeof(TObject);
            // Observing on value types is nonsensical; we can also omit trying for sealed reference types that do not
            // implement INPC (as there cannot be a derived type that does implement it).
            TryObserveOnTObject = !type.IsValueType &&
                                  (typeof(INotifyPropertyChanged).IsAssignableFrom(type) || !type.IsSealed);
        }

        public InstanceMemberBinding(Binding<TObject> obj, string name, Func<TObject, TResult> accessor)
            : this(obj, name, accessor, BindingMode.Default)
        {
        }

        private InstanceMemberBinding(Binding<TObject> obj, string name, Func<TObject, TResult> accessor, BindingMode mode)
        {
            _mode = mode;
            _object = obj;
            _name = name;
            _accessor = accessor;
            _object.ResultChanged += OnObjectResultChanged;
            OnObjectResultChanged(_object, BindingResultChange.Create(None<TObject>(), _object.Result));
        }

        public override Binding<TResult> Clone(BindingCloneContext context)
        {
            return new InstanceMemberBinding<TObject, TResult>(context.Clone(_object), _name, _accessor, context.BindingMode);
        }

        private void OnObjectResultChanged(object _, BindingResultChange<TObject> e)
        {
            if (TryObserveOnTObject && _mode == BindingMode.Default)
            {
                EventUtils.Move(OnObjectPropertyChanged, e.OldResult.GetValueOrDefault(), e.NewResult.GetValueOrDefault());
            }
            MaybeOnResultChanged();
        }

        private void OnObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var name = e.PropertyName;
            if (string.Equals(name, _name) || string.IsNullOrEmpty(name))
            {
                MaybeOnResultChanged();
            }
        }

        private void MaybeOnResultChanged()
        {
            MaybeOnResultChanged(Evaluate());
        }

        private BindingResult<TResult> Evaluate()
        {
            var obj = _object.Result;
            return obj.HasValue && obj._value != null ? Some(_accessor(obj._value)) : None<TResult>();
        }
    }
}
