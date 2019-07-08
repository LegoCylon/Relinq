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

    public static class HashSetExtensions {
        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<HashSetEnumerator<TSource>, TSource> AsEnumerable<TSource> (
            this HashSet<TSource> hashSet
        ) => new EnumerableAdapter<HashSetEnumerator<TSource>, TSource>(
            enumerator:new HashSetEnumerator<TSource>(hashSet:hashSet)
        );
    }
    
}