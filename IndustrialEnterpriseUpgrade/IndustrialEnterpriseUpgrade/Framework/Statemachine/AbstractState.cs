namespace IndustrialEnterpriseUpgrade.Framework.Statemachine {
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TFokus"></typeparam>
	public abstract class AbstractState<TFokus> {
		public AbstractState(TFokus Fokus) : base() {
			if(Fokus != null) {
				this.Fokus = Fokus;
			}
		}
		/// <summary>
		/// Das Objekt vom dem die Maschine seinen Status bezieht.
		/// </summary>
		public TFokus Fokus {
			get;
			set;
		}
		/// <summary>
		/// Der aktuelle Status der Maschine besorgt sich vom <see cref="Fokus"/> die aktuell benötigten Informationen.
		/// </summary>
		public abstract void PullCurrentState();
		/// <summary>
		/// Es werden Aktionen durchgeführt, basierend auf dem Aktuellen Status der Maschine und des <see cref="Fokus"/>.
		/// </summary>
		public abstract void WorkWithCurrentState();
		/// <summary>
		/// Es werden Aktionen durchgeführt, die unabhängig vom Zustand der Maschine sind, aber abhänging sind vom Zustand des <see cref="Fokus"/>.
		/// </summary>
		public abstract void CommonWork();
		/// <summary>
		/// Auf Basis des aktuellen Status der Maschine und dem aktuellen Zustand vom <see cref="Fokus"/>, wird entschieden, ob ein Statuswechsel notwendig ist, oder nicht.
		/// </summary>
		/// <returns><code>this</code>, wenn ein Wechsel nicht notwendig ist,
		/// ansonsten ein neues Objekt vom Typ <code>AbstractState</code>, welches am besten für den aktuellen Zustand vom <see cref="Fokus"/> geeignet ist.</returns>
		public abstract AbstractState<TFokus> NextState();
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual AbstractState<TFokus> Execute() {
			PullCurrentState();
			//Vielleicht ist ja ein anderer Status jetzt besser?
			AbstractState<TFokus> next = NextState();
			if(next != this) {
				next.PullCurrentState();
			}
			next.CommonWork();
			next.WorkWithCurrentState();
			return next;
		}
	}
}