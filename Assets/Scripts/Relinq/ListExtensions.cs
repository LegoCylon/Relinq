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

    public static class ListExtensions {
        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<ListEnumerator<TSource>, TSource> AsEnumerable<TSource> (
            this List<TSource> list
        ) => ListEnumerator<TSource>.GetEnumerable(list:list);

        //--------------------------------------------------------------------------------------------------------------
        public static void Add<TEnumerator, TSource> (
            this List<TSource> list,
            EnumerableAdapter<TEnumerator, TSource> enumerable
        ) 
            where TEnumerator : IAdaptedEnumerator<TSource>
            => list.AddRange(enumerable:enumerable);
        
        //--------------------------------------------------------------------------------------------------------------
        public static void AddRange<TEnumerator, TSource> (
            this List<TSource> list,
            EnumerableAdapter<TEnumerator, TSource> enumerable
        )
            where TEnumerator : IAdaptedEnumerator<TSource>
        {
            foreach (var element in enumerable) {
                list.Add(item:element);
            }
        }
    }
    
}