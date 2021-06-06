using System;
using System.Collections.Generic;
using System.Text;

namespace Nonograms
{
    class FSM
    {
        private List<FSMNode> data;

        public FSM(List<FSMNode> data)
        {
            this.data = new List<FSMNode>(data);
        }

        public FSM()
        {
            data = new List<FSMNode>();
        }

        public FSMNode this[int index] 
        {
            get
            {
                return data[index];
            }
            set
            {
                data[index] = value;
            }
        }

        public int Count { get => data.Count; }

        public void Add(FSMNode fSMNode)
        {
            data.Add(fSMNode);
        }
    }
}
