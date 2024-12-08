namespace RTG
{
    public class CapsuleCollider3DChangedAction : IUndoRedoAction
    {
        private CapsuleCollider3DSnapshot _preChangeSnapshot;
        private CapsuleCollider3DSnapshot _postChangeSnapshot;

        public CapsuleCollider3DChangedAction(CapsuleCollider3DSnapshot preChangeSnapshot, CapsuleCollider3DSnapshot postChangeSnapshot)
        {
            _preChangeSnapshot = new CapsuleCollider3DSnapshot(preChangeSnapshot);
            _postChangeSnapshot = new CapsuleCollider3DSnapshot(postChangeSnapshot);
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
