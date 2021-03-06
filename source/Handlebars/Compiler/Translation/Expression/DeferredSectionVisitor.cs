﻿using System;
using System.Linq.Expressions;
using System.Collections;
using System.Linq;
using System.IO;
using System.Reflection;

namespace HandlebarsDotNet.Compiler
{
    internal class DeferredSectionVisitor : HandlebarsExpressionVisitor
    {
        public static Expression Bind(Expression expr, CompilationContext context)
        {
            return new DeferredSectionVisitor(context).Visit(expr);
        }

        private DeferredSectionVisitor(CompilationContext context)
            : base(context)
        {
        }

        protected override Expression VisitDeferredSectionExpression(DeferredSectionExpression dsex)
        {
            Action<object, BindingContext, Action<TextWriter, object>> method;
            if (dsex.Path.Path.StartsWith("#"))
            {
                method = RenderSection;
            }
            else if (dsex.Path.Path.StartsWith("^"))
            {
                method = RenderEmptySection;
            }
            else
            {
                throw new HandlebarsCompilerException("Tried to compile a section expression that did not begin with # or ^");
            }
            return Expression.Call(
                method.GetMethodInfo(),
                HandlebarsExpression.Path(dsex.Path.Path.Substring(1)),
                CompilationContext.BindingContext,
                new FunctionBuilder(CompilationContext.Configuration).Compile(dsex.Body, CompilationContext.BindingContext));
        }

        private static void RenderEmptySection(object value, BindingContext context, Action<TextWriter, object> template)
        {
            if (value is bool && (bool)value == false)
            {
                template(context.TextWriter, context);
            }
            else if (HandlebarsUtils.IsFalsyOrEmpty(value) == true)
            {
                template(context.TextWriter, value);
            }
        }

        private static void RenderSection(object value, BindingContext context, Action<TextWriter, object> template)
        {
            if (value is bool && (bool)value == true)
            {
                template(context.TextWriter, context);
            }
            else if (HandlebarsUtils.IsFalsyOrEmpty(value))
            {
                return;
            }
            else if (value is IEnumerable)
            {
                foreach (var item in ((IEnumerable)value))
                {
                    template(context.TextWriter, item);
                }
            }
            else if (value != null)
            {
                template(context.TextWriter, value);
            }
            else
            {
                throw new HandlebarsRuntimeException("Could not render value for the section");
            }
        }
    }
}

