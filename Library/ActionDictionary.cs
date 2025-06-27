using System;
using System.Collections.Generic;
namespace SEV.Library
{
    public class TypeLess
    {
        private void ThisIsAFooClass(){}
    }

    public class ActionDictionary<T>
    {
        private Dictionary<T, List<Action>> Handlers { get; set; }
        private Action DefaultHandler { get; set; }
        private List<FilterHandler<T>> Filters {get; set;}
        private bool IsExclusive {get; set;}

        public ActionDictionary()
        {
            DefaultHandler = null;
            Handlers = new Dictionary<T, List<Action>>();
            Filters = new List<FilterHandler<T>>();
            IsExclusive = false;
        }

        public ActionDictionary<T> ExclusivelyMode(){
            IsExclusive = true;
            return this;
        }

        public static ActionDictionary<T> Create()
        {
            return new ActionDictionary<T>();
        }

        public ActionDictionary<T> When( T Value, Action Handler)
        {
            if (!this.Handlers.ContainsKey(Value))
            {
                List<Action> CurrentHandler = new List<Action>();
                CurrentHandler.Add(Handler);
                this.Handlers.Add(Value, CurrentHandler);
            }
            else
            {
                this.Handlers[Value].Add(Handler);
            }
            return this;
        }

        public ActionDictionary<T> When( T[] Values, Action Handler)
        {
            Array.ForEach(Values, (el) =>
                {
                    if (!this.Handlers.ContainsKey(el))
                    {
                        List<Action> CurrentHandler = new List<Action>();
                        CurrentHandler.Add(Handler );
                        this.Handlers.Add(el, CurrentHandler);
                    }
                    else
                    {
                        this.Handlers[el].Add(Handler);
                    }
                }
            );
            return this;
        }

        public ActionDictionary<T> When( Func<T,bool> Filter, Action Handler)
        {
            FilterHandler<T> CurrentFilter = new FilterHandler<T>();
            CurrentFilter.Filter = Filter;
            CurrentFilter.Handler = Handler;
            Filters.Add(CurrentFilter);
            return this;
        }

        public ActionDictionary<T> Default(Action Handler)
        {
            DefaultHandler = Handler;
            return this;
        }

        public bool Evaluate(T Value)
        {
            bool Result = false;
            Filters.ForEach(
                f =>
                {
                    if(! (IsExclusive && Result))
                    {
                        if (f.Filter(Value))
                        {
                            f.Handler();
                            Result = true;
                        }
                    }
                }
            );
            if (Value != null)
            {
                if (Handlers.ContainsKey(Value))
                {
                    if(! (IsExclusive && Result)){
                        Handlers[Value].ForEach(hdlr => hdlr());
                        Result = true;
                    }
                }
            }
            if(!Result && DefaultHandler != null)
            {
                DefaultHandler();
                Result = true;
            }
            return Result;
        }

    }

    public class FilterHandler<T>
    {
        public Func<T,bool> Filter { get; set; }
        public Action Handler { get; set; }
    }
}
