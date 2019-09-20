// Copyright (c) 2019 Timothé Lapetite - nebukam@gmail.com.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

namespace Nebukam.Slate
{
    public static class SlotNeighbor
    {

        // LEFT/RIGHT is over X
        public static IntTrio LEFT = new IntTrio(1, 0, 0);
        public static IntTrio RIGHT = new IntTrio(-1, 0, 0);

        // UP/DOWN is over Y
        public static IntTrio UP = new IntTrio(0, 1, 0);
        public static IntTrio DOWN = new IntTrio(0, -1, 0);

        // FRONT/BACK is over Z
        public static IntTrio FRONT = new IntTrio(0, 0, 1);
        public static IntTrio BACK = new IntTrio(0, 0, -1);

    }
}
