#region Copyright (c) 2011, EventDay Inc.

// Copyright (c) 2011, EventDay Inc.
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the EventDay Inc. nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL EventDay Inc. BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;

using Autofac;
using Autofac.Core;

namespace Lokad.Cqrs.Extensions.EventStore.Build
{
    public sealed class SnapshotModule : IModule
    {
        private readonly ContainerBuilder builder = new ContainerBuilder();
        private TimeSpan checkInterval = TimeSpan.FromMinutes(10);
        private int maxThreshold = 250;
        private bool enabled;

        public SnapshotModule()
        {
            Disable();
        }

        #region Implementation of IModule

        void IModule.Configure(IComponentRegistry componentRegistry)
        {
            if (!enabled)
                return;

            builder.RegisterType<SnapshottingProcess>()
                .WithParameters(new[]
                {
                    TypedParameter.From(maxThreshold),
                    TypedParameter.From(checkInterval)
                }).SingleInstance()
                .As<IEngineProcess>();

            builder.Update(componentRegistry);
        }

        #endregion

        public SnapshotModule Enable()
        {
            enabled = true;
            return this;
        }

        public SnapshotModule Disable()
        {
            enabled = false;
            return this;
        }

        public SnapshotModule MaxThreshold(int threshold)
        {
            maxThreshold = threshold;
            return this;
        }

        public SnapshotModule CheckEvery(TimeSpan interval)
        {
            checkInterval = interval;
            return this;
        }
    }
}