namespace FQ.BehaviourProviders
{
    /// <summary>
    /// Defines the Game Elements at a top level which make up the Snake Game.
    /// </summary>
    public enum SnakeGameElements
    {
        /// <summary>
        /// A controllable actor.
        /// </summary>
        Player,
        
        /// <summary>
        /// Obstetrical and barriers.
        /// </summary>
        Level,
        
        /// <summary>
        /// Collectables.
        /// </summary>
        Food,
        
        /// <summary>
        /// User Interface in the world.
        /// </summary>
        Hud,
        
        /// <summary>
        /// User interface which is directly interactable.
        /// </summary>
        Menu,
    }
}