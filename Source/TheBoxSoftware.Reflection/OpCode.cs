
namespace TheBoxSoftware.Reflection
{
    public struct OpCode
    {
        internal string m_stringname;
        internal StackBehaviour m_pop;
        internal StackBehaviour m_push;
        internal OperandType m_operand;
        internal OpCodeType m_type;
        internal int m_size;
        internal byte m_s1;
        internal byte m_s2;
        internal FlowControl m_ctrl;
        internal bool m_endsUncondJmpBlk;
        internal int m_stackChange;

        internal OpCode(string stringname,
                        StackBehaviour pop,
                        StackBehaviour push,
                        OperandType operand,
                        OpCodeType type,
                        int size,
                        byte s1,
                        byte s2,
                        FlowControl ctrl,
                        bool endsjmpblk,
                        int stack)
        {
            m_stringname = stringname;
            m_pop = pop;
            m_push = push;
            m_operand = operand;
            m_type = type;
            m_size = size;
            m_s1 = s1;
            m_s2 = s2;
            m_ctrl = ctrl;
            m_endsUncondJmpBlk = endsjmpblk;
            m_stackChange = stack;
        }

        internal bool EndsUncondJmpBlk()
        {
            return m_endsUncondJmpBlk;
        }

        internal int StackChange()
        {
            return m_stackChange;
        }

        public OperandType OperandType
        {
            get
            {
                return m_operand;
            }
        }

        public FlowControl FlowControl
        {
            get
            {
                return m_ctrl;
            }
        }

        public OpCodeType OpCodeType
        {
            get
            {
                return m_type;
            }
        }

        public StackBehaviour StackBehaviourPop
        {
            get
            {
                return m_pop;
            }
        }

        public StackBehaviour StackBehaviourPush
        {
            get
            {
                return m_push;
            }
        }

        public int Size
        {
            get
            {
                return m_size;
            }
        }

        public short Value
        {
            get
            {
                if(m_size == 2)
                {
                    return (short)((m_s1 << 8) | m_s2);
                }
                return m_s2;
            }
        }

        public string Name
        {
            get
            {
                return m_stringname;
            }
        }

        public override bool Equals(object obj)
        {
            return ((obj is OpCode) && this.Equals((OpCode)obj));
        }

        public bool Equals(OpCode obj)
        {
            return ((obj.m_s1 == m_s1) && (obj.m_s2 == m_s2));
        }

        public static bool operator ==(OpCode a, OpCode b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(OpCode a, OpCode b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return m_stringname.GetHashCode();
        }

        public override string ToString()
        {
            return m_stringname;
        }
    }
}