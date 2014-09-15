	//-----------------------------------------------------------------------//
	// <copyright file="IScreenFactory.cs" company="A.D.F.Software">
	// Copyright "A.D.F.Software" (c) 2014 All Rights Reserved
	// <author>Andrea M. Falappi</author>
	// <date>Wednesday, September 24, 2014 11:36:49 AM</date>
	// </copyright>
	//
	// * NOTICE:  All information contained herein is, and remains
	// * the property of Andrea M. Falappi and its suppliers,
	// * if any.  The intellectual and technical concepts contained
	// * herein are proprietary to A.D.F.Software
	// * and its suppliers and may be covered by World Wide and Foreign Patents,
	// * patents in process, and are protected by trade secret or copyright law.
	// * Dissemination of this information or reproduction of this material
	// * is strictly forbidden unless prior written permission is obtained
	// * from Andrea M. Falappi.
	//-----------------------------------------------------------------------//

using System;
namespace PrinceOfPersia
{
    /// <summary>
    /// Defines an object that can create a screen when given its type.
    /// 
    /// The ScreenManager attempts to handle tombstoning on Windows Phone by creating an XML
    /// document that has a list of the screens currently in the manager. When the game is
    /// reactivated, the ScreenManager needs to create instances of those screens. However
    /// since there is no restriction that a particular GameScreen subclass has a parameterless
    /// constructor, there is no way the ScreenManager alone could create those instances.
    /// 
    /// IScreenFactory fills this gap by providing an interface the game should implement to
    /// act as a translation from type to instance. The ScreenManager locates the IScreenFactory
    /// from the Game.Services collection and passes each screen type to the factory, expecting
    /// to get the correct GameScreen out.
    /// 
    /// If your game screens all have parameterless constructors, the minimal implementation of
    /// this interface would look like this:
    /// 
    /// return Activator.CreateInstance(screenType) as GameScreen;
    /// 
    /// If you have screens with constructors that take arguments, you will need to ensure that
    /// you can read these arguments from storage or generate new ones, then construct the screen
    /// based on the type.
    /// 
    /// The ScreenFactory type in the sample game has the minimal implementation along with some
    /// extra comments showing a potentially more complex example of how to implement IScreenFactory.
    /// </summary>
    public interface IScreenFactory
    {
        /// <summary>
        /// Creates a GameScreen from the given type.
        /// </summary>
        /// <param name="screenType">The type of screen to create.</param>
        /// <returns>The newly created screen.</returns>
        GameScreen CreateScreen(Type screenType);
    }
}
