//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.IO;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Input.Touch;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Linq;
//using System.Reflection;
//using Microsoft.Xna.Framework.Content.Pipeline;


//namespace PrinceOfPersia
//{

//    [ContentProcessor(DisplayName = "ExternalRefObjectContentProcessor")]
//    public class ExternalRefObjectContentProcessor : ContentProcessor<object, object>
//    {
//        private void ReplaceReferences(object input, ContentProcessorContext context)
//        {
//            Func<ExternalReference<object>, string, object> BuildAssetMethodTemplate = context.BuildAsset<object, object>;
//            var BuildAssetMethod = BuildAssetMethodTemplate.Method.GetGenericMethodDefinition();

//            foreach (var field in input.GetType().GetFields().Where(f => !f.IsStatic && !f.IsLiteral))
//            {
//                Type fieldType = field.FieldType;
//                object fieldValue = field.GetValue(input);

//                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(ExternalReference<>))
//                {
//                    var GenericBuildMethod = BuildAssetMethod.MakeGenericMethod(fieldType.GetGenericArguments().First(), fieldType.GetGenericArguments().First());

//                    object BuiltObject;

//                    try
//                    {
//                        BuiltObject = GenericBuildMethod.Invoke(context, new object[] { fieldValue, null });
//                    }
//                    catch (Exception Ex)
//                    {
//                        throw Ex.InnerException;
//                    }

//                    field.SetValue(input, BuiltObject);
//                }
//                else if (fieldValue is IEnumerable && !(fieldValue is string))
//                {
//                    foreach (var item in (fieldValue as IEnumerable))
//                    {
//                        ReplaceReferences(item, context);
//                    }
//                }
//                else if (fieldValue != null && !(fieldValue is string))
//                {
//                    ReplaceReferences(fieldValue, context);
//                }
//            }

//            foreach (var property in input.GetType().GetProperties().Where(p => p.CanRead && p.CanWrite))
//            {
//                Type propertyType = property.PropertyType;
//                object propertyValue = property.GetValue(input, null);

//                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(ExternalReference<>))
//                {
//                    var GenericBuildMethod = BuildAssetMethod.MakeGenericMethod(propertyType.GetGenericArguments().First(), propertyType.GetGenericArguments().First());

//                    object BuiltObject;

//                    try
//                    {
//                        BuiltObject = GenericBuildMethod.Invoke(context, new object[] { property.GetValue(input, null), null });
//                    }
//                    catch (Exception Ex)
//                    {
//                        throw Ex.InnerException;
//                    }
//                    property.SetValue(input, BuiltObject, null);
//                }
//                else if (propertyValue is IEnumerable && !(propertyValue is string))
//                {
//                    foreach (var item in (propertyValue as IEnumerable))
//                    {
//                        ReplaceReferences(item, context);
//                    }
//                }
//                else if (propertyValue != null && !(propertyValue is string))
//                {
//                    ReplaceReferences(propertyValue, context);
//                }
//            }
//        }

//        public override object Process(object input, ContentProcessorContext context)
//        {
//            ReplaceReferences(input, context);

//            return input;
//        }
//    }
//}