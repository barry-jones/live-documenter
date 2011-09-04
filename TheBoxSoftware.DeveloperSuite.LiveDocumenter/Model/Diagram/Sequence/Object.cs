using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model.Diagram.SequenceDiagram {
	/// <summary>
	/// Represents an object instance or static in a sequence diagram.
	/// </summary>
	public class Object {
		private static Dictionary<string, Object> objects = new Dictionary<string, Object>();

		private Object(string name, string type) {
			this.Name = name;
			this.Type = type;
			this.Activations = new List<Activation>();
			objects.Add(name, this);
		}

		public static Object Create(string name, string type) {
			Object o;
			if (objects.ContainsKey(name)) {
				o = objects[name];
			}
			else {
				o = new Object(name, type);
			}
			return o;
		}

		public Activation RecieveCall(Call call) {
			Activation activation = new Activation();
			activation.Recieved = call;
			this.Activations.Add(activation);
			return activation;
		}

		public static void Clear(){
			objects.Clear();
		}

		public static List<Object> GetObjects() {
			List<Object> calledInOrder = new List<Object>();
			foreach (KeyValuePair<string, Object> current in objects) {
				calledInOrder.Add(current.Value);
			}
			return calledInOrder;
		}

		/// <summary>
		/// Indicates if this is a static or instance object.
		/// </summary>
		public bool IsStatic { get; set; }

		/// <summary>
		/// Gets or sets the name of the object.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the name of the type of object.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// The list of activations for this object.
		/// </summary>
		public List<Activation> Activations { get; set; }
	}
}
