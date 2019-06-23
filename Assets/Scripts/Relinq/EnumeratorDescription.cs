//======================================================================================================================
//  Copyright 2019 Andy Bond (LegoCylon)
//  
//  Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with
//  the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on
//  an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and limitations under the License.
//======================================================================================================================

using System;

namespace Relinq {
    
    public class EnumeratorDescription<TEnumerator, TSource> : 
        IEnumeratorDescription<TEnumerator, TSource>
        where TEnumerator : IAdaptedEnumerator<TEnumerator, TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Types
        //--------------------------------------------------------------------------------------------------------------
        public delegate TSource CurrentDelegate (ref TEnumerator enumerator); 
        public delegate void DisposeDelegate (ref TEnumerator enumerator); 
        public delegate bool MoveNextDelegate (ref TEnumerator enumerator); 
        public delegate void ResetDelegate (ref TEnumerator enumerator);

        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public CurrentDelegate Current { get; }
        public DisposeDelegate Dispose { get; }
        public MoveNextDelegate MoveNext { get; }
        public ResetDelegate Reset { get; }

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public EnumeratorDescription (
            CurrentDelegate current,
            DisposeDelegate dispose,
            MoveNextDelegate moveNext,
            ResetDelegate reset
        ) {
            Current = current ?? throw new ArgumentNullException(paramName:nameof(current));
            Dispose = dispose ?? throw new ArgumentNullException(paramName:nameof(dispose));
            MoveNext = moveNext ?? throw new ArgumentNullException(paramName:nameof(moveNext));
            Reset = reset ?? throw new ArgumentNullException(paramName:nameof(reset));
        }
    }
    
}