// -----------------------------------------------------------------------
// <copyright file="Event{T}.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Features
{
    using System;

    using Exiled.API.Features;
    using Exiled.Events.Interfaces;

    /// <summary>
    /// The custom <see cref="EventHandler"/> delegate.
    /// </summary>
    /// <typeparam name="T">The <see cref="EventHandler{TEventArgs}"/> type.</typeparam>
    /// <param name="ev">The <see cref="EventHandler{TEventArgs}"/> instance.</param>
    public delegate void CustomEventHandler<T>(T ev)
        where T : EventArgs;

    /// <summary>
    /// An implementation of the <see cref="IEvent"/> interface that encapsulates an event with arguments.
    /// </summary>
    /// <typeparam name="T">The specified <see cref="EventArgs"/> that the event will use.</typeparam>
    public class Event<T> : IEvent
        where T : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event{T}"/> class.
        /// </summary>
        internal Event()
        {
        }

        private event CustomEventHandler<T> InnerEvent;

        /// <summary>
        /// Subscribes a target <see cref="CustomEventHandler{TEventArgs}"/> to the inner event and checks if patching is possible, if dynamic patching is enabled.
        /// </summary>
        /// <param name="event">The <see cref="Event{T}"/> the <see cref="CustomEventHandler{T}"/> will be subscribed to.</param>
        /// <param name="handler">The <see cref="CustomEventHandler{T}"/> that will be subscribed to the <see cref="Event{T}"/>.</param>
        /// <returns>The <see cref="Event{T}"/> with the handler subscribed to it.</returns>
        public static Event<T> operator +(Event<T> @event, CustomEventHandler<T> handler)
        {
            @event.Subscribe(handler);
            return @event;
        }

        /// <summary>
        /// Unsubscribes a target <see cref="CustomEventHandler{TEventArgs}"/> from the inner event and checks if unpatching is possible, if dynamic patching is enabled.
        /// </summary>
        /// <param name="event">The <see cref="Event{T}"/> the <see cref="CustomEventHandler{T}"/> will be unsubscribed from.</param>
        /// <param name="handler">The <see cref="CustomEventHandler{T}"/> that will be unsubscribed from the <see cref="Event{T}"/>.</param>
        /// <returns>The <see cref="Event{T}"/> with the handler unsubscribed from it.</returns>
        public static Event<T> operator -(Event<T> @event, CustomEventHandler<T> handler)
        {
            @event.Unsubscribe(handler);
            return @event;
        }

        /// <summary>
        /// Subscribes a target <see cref="CustomEventHandler{T}"/> to the inner event if the conditional is true.
        /// </summary>
        /// <param name="handler">The handler to add.</param>
        /// <param name="shouldSubscribe">Whether the handler should be subscribed or not.</param>
        public void Subscribe(CustomEventHandler<T> handler, bool shouldSubscribe = true)
        {
            if (shouldSubscribe)
            {
                if (Events.Instance.Config.UseDynamicPatching && InnerEvent is null)
                    Events.Instance.Patcher.Patch(this);

                InnerEvent += handler;
            }
        }

        /// <summary>
        /// Unsubscribes a target <see cref="CustomEventHandler{T}"/> from the inner event if the conditional is true.
        /// </summary>
        /// <param name="handler">The handler to add.</param>
        /// <param name="shouldUnsubscribe">Whether the handler should be subscribed or not.</param>
        public void Unsubscribe(CustomEventHandler<T> handler, bool shouldUnsubscribe = true)
        {
            if (shouldUnsubscribe)
                InnerEvent -= handler;
        }

        /// <summary>
        /// Executes all <see cref="CustomEventHandler{TEventArgs}"/> listeners safely.
        /// </summary>
        /// <param name="arg">The event arg/>.</param>
        /// <exception cref="ArgumentNullException">Event or its arg is <see langword="null"/>.</exception>
        public void InvokeSafely(T arg)
        {
            if (InnerEvent is null)
                return;

            foreach (CustomEventHandler<T> handler in InnerEvent.GetInvocationList())
            {
                try
                {
                    handler(arg);
                }
                catch (Exception ex)
                {
                    Log.Error($"Method \"{handler.Method.Name}\" of the class \"{handler.Method.ReflectedType.FullName}\" caused an exception when handling the event \"{GetType().FullName}\"");
                    Log.Error(ex);
                }
            }
        }
    }
}
