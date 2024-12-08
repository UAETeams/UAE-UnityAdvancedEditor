namespace RTG
{
    public class CharacterController3DChangedAction : IUndoRedoAction
    {
        private CharacterController3DSnapshot _preChangeSnapshot;
        private CharacterController3DSnapshot _postChangeSnapshot;

        public CharacterController3DChangedAction(CharacterController3DSnapshot preChangeSnapshot, CharacterController3DSnapshot postChangeSnapshot)
        {
            _preChangeSnapshot = new CharacterController3DSnapshot(preChangeSnapshot);
            _postChangeSnapshot = new CharacterController3DSnapshot(postChangeSnapshot);
        }

        public void Execute()
        {
            RTUndoRedo.Get.RecordAction(this);
        }

        public void OnRemovedFromUndoRedoStack()
        {
        }

        public void Redo()
        {
            _postChangeSnapshot.Apply();
        }

        public void Undo()
        {
            _preChangeSnapshot.Apply();
        }
    }
}
