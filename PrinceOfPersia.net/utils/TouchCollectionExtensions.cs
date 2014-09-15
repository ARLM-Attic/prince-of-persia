	//-----------------------------------------------------------------------//
	// <copyright file="TouchCollectionExtensions.cs" company="A.D.F.Software">
	// Copyright "A.D.F.Software" (c) 2014 All Rights Reserved
	// <author>Andrea M. Falappi</author>
	// <date>Wednesday, September 24, 2014 11:36:49 AM</date>
	// </copyright>
	//
    // * NOTICE:  This file and all PrinceOfPersia.net sources are under 
    // * the Apache 2.0 license for details see :
    // * Version 2.0, January 2004
    // * http://www.apache.org/licenses/
    //-----------------------------------------------------------------------//

using Microsoft.Xna.Framework.Input.Touch;

namespace PrinceOfPersia
{
    /// <summary>
    /// Provides extension methods for the TouchCollection type.
    /// </summary>
    public static class TouchCollectionExtensions
    {
        /// <summary>
        /// Determines if there are any touches on the screen.
        /// </summary>
        /// <param name="touchState">The current TouchCollection.</param>
        /// <returns>True if there are any touches in the Pressed or Moved state, false otherwise</returns>
        public static bool AnyTouch(this TouchCollection touchState)
        {
            foreach (TouchLocation location in touchState)
            {
                if (location.State == TouchLocationState.Pressed || location.State == TouchLocationState.Moved)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
