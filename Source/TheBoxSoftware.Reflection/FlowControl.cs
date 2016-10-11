using System;

namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// Describes how the instruction effects the flow of execution.
    /// </summary>
	[Serializable]
    public enum FlowControl
    {
        Branch = 0,
        Break = 1,
        Call = 2,
        Cond_Branch = 3,
        Meta = 4,
        Next = 5,
        [Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
        Phi = 6,
        Return = 7,
        Throw = 8
    }
}