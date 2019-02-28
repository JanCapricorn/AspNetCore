// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Contains extension methods for <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    public static class FallbackEndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Adds a specialized <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that will match
        /// requests for non-filenames with a very low priority.
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <param name="requestDelegate">The delegate executed when the endpoint is matched.</param>
        /// <returns>A <see cref="IEndpointConventionBuilder"/> that can be used to further customize the endpoint.</returns>
        /// <remarks>
        /// <para>
        /// <see cref="MapFallback(IEndpointRouteBuilder, RequestDelegate)"/> is intended to handle cases where URL path of
        /// the request does not contain a filename, and no other endpoint has matched. This is convenient for routing
        /// requests for dynamic content to a SPA framework, while also allowing requests for non-existent files to
        /// result in an HTTP 404.
        /// </para>
        /// <para>
        /// <see cref="MapFallback(IEndpointRouteBuilder, RequestDelegate)"/> registers an endpoint using the pattern
        /// <c>{*path:nonfile}</c>. The order of the registered endpoint will be <c>int.MaxValue</c>.
        /// </para>
        /// </remarks>
        public static IEndpointConventionBuilder MapFallback(this IEndpointRouteBuilder builder, RequestDelegate requestDelegate)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (requestDelegate == null)
            {
                throw new ArgumentNullException(nameof(requestDelegate));
            }

            return builder.MapFallback("{*path:nonfile}", requestDelegate);
        }

        /// <summary>
        /// Adds a specialized <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that will match
        /// the provided pattern with a very low priority.
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <param name="pattern">The route pattern.</param>
        /// <param name="requestDelegate">The delegate executed when the endpoint is matched.</param>
        /// <returns>A <see cref="IEndpointConventionBuilder"/> that can be used to further customize the endpoint.</returns>
        /// <remarks>
        /// <para>
        /// <see cref="MapFallback(IEndpointRouteBuilder, string, RequestDelegate)"/> is intended to handle cases where no
        /// other endpoint has matched. This is convenient for routing requests to a SPA framework.
        /// </para>
        /// <para>
        /// The order of the registered endpoint will be <c>int.MaxValue</c>.
        /// </para>
        /// <para>
        /// This overload will use the provided <paramref name="pattern"/> verbatim. Use the <c>:nonfile</c> route contraint
        /// to exclude requests for static files.
        /// </para>
        /// </remarks>
        public static IEndpointConventionBuilder MapFallback(
            this IEndpointRouteBuilder builder,
            string pattern,
            RequestDelegate requestDelegate)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (requestDelegate == null)
            {
                throw new ArgumentNullException(nameof(requestDelegate));
            }

            var coventionBuilder = builder.Map(pattern, "Fallback", requestDelegate);
            coventionBuilder.Add(b => ((RouteEndpointBuilder)b).Order = int.MaxValue);
            return coventionBuilder;
        }
    }
}
