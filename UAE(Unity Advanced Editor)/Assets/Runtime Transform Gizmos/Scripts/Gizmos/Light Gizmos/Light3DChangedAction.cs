namespace RTG
{
    public class Light3DChangedAction : IUndoRedoAction
    {
        private Light3DSnapshot _preChangeSnapshot;
        private Light3DSnapshot _postChangeSnapshot;

        public Light3DChangedAction(Light3DSnapshot preChangeSnapshot, Light3DSnapshot postChangeSnapshot)
        {
            _preChangeSnapshot = new Light3DSnapshot(preChangeSnapshot);
            _postChangeSnapshot = new Light3DSnapshot(postChangeSnapshot);
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
