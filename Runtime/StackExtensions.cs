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

using System.Collections.Generic;

namespace Relinq {

    public static class StackExtensions {
        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<StackEnumerator<TSource>, TSource> AsEnumerable<TSource> (
            this Stack<TSource> stack
        ) => new EnumerableAdapter<StackEnumerator<TSource>, TSource>(
            enumerator:new StackEnumerator<TSource>(stack:stack)
        );

        //--------------------------------------------------------------------------------------------------------------
        public static void Add<TSource> (this Stack<TSource> stack, TSource element) => stack.Push(item:element);

        //--------------------------------------------------------------------------------------------------------------
        public static void PushRange<TEnumerator, TSource> (
            this Stack<TSource> stack,
            EnumerableAdapter<TEnumerator, TSource> enumerable
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            foreach (var element in enumerable) {
                stack.Push(item:element);
            }
        }
    }
    
}
