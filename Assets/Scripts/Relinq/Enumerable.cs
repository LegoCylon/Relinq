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
    
    public static class Enumerable {
        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<CountEnumerator<TSource>, TSource> Empty<TSource> () =>
            Range(start:default(TSource), count:0, generator:(start, index) => throw new InvalidOperationException())
        ;

        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<CountEnumerator<int>, int> Range (int start, int count) => 
            Range(start:start, count:count, generator:(begin, index) => begin + index)
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<CountEnumerator<TSource>, TSource> Range<TSource> (
            TSource start, 
            int count, 
            Func<TSource, int, TSource> generator
        ) => CountEnumerator<TSource>.GetEnumerable(start:start, count:count, generator:generator);

        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<RepeatEnumerator<TSource>, TSource> Repeat<TSource> (
            TSource element, 
            int count
        ) => RepeatEnumerator<TSource>.GetEnumerable(element:element, count:count);
    }
    
}