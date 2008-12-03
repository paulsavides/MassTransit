// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Pipeline
{
	using System.Collections.Generic;
	using Interceptors.Outbound;

	public class OutboundPipeline :
		PipelineBase<IOutboundInterceptor>
	{
		public OutboundPipeline(IObjectBuilder builder)
			: base(builder)
		{
		}

		public IEnumerable<IEndpoint> Publish<TMessage>(TMessage message) where TMessage : class
		{
			var context = new OutboundContext(Pipeline);

			_interceptors.ForEach(interceptor =>
				{
					foreach (IEndpoint endpoint in interceptor.Publish(context, message))
					{
						context.AddEndpointToPublish(endpoint);
					}
				});

			return context.GetEndpoints();
		}
	}
}