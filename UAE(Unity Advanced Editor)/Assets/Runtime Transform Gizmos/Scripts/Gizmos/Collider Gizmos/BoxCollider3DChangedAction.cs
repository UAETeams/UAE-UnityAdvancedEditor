namespace RTG
{
    public class BoxCollider3DChangedAction : IUndoRedoAction
    {
        private BoxCollider3DSnapshot _preChangeSnapshot;
        private BoxCollider3DSnapshot _postChangeSnapshot;

        public BoxCollider3DChangedAction(BoxCollider3DSnapshot preChangeSnapshot, BoxCollider3DSnapshot postChangeSnapshot)
        {
            _preChangeSnapshot = new BoxCollider3DSnapshot(preChangeSnapshot);
            _postChangeSnapshot = new BoxCollider3DSnapshot(postChangeSnapshot);
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
