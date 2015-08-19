namespace Chain
{
	public interface IHandler<T>
	{
		void Handle(T obj);
	
		void SetNext(IHandler<T> obj);
	
		void Append(IHandler<T> handler);
	}
	
	
	public abstract class Handler<T> : IHandler<T>
	{
		protected abstract bool CanHandle(T item);
	
		protected abstract  void HandleImpl(T item);
	
		public void Handle(T obj)
		{
			if (CanHandle(obj))
				HandleImpl(obj);
			else
				PassToNext(obj);
		}
	
		private void PassToNext(T obj)
		{
			if (next == null)
				return;
			next.Handle(obj);
		}
	
		public void SetNext(IHandler<T> obj)
		{
			next = obj;
		}
	
		public void Append(IHandler<T> handler)
		{
			if(next == null)
				SetNext(handler);
			else
				next.Append(handler);
		}
	
		private IHandler<T> next;
	}
}