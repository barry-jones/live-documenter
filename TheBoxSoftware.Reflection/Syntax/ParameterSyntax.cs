using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	using TheBoxSoftware.Reflection.Signitures;

	public class ParameterSyntax {
		private ParamDef param;
		private ParamSignitureToken signitureToken;

		public ParameterSyntax(ParamDef param, ParamSignitureToken signitureToken) {
			this.param = param;
			this.signitureToken = signitureToken;
		}
	}
}
