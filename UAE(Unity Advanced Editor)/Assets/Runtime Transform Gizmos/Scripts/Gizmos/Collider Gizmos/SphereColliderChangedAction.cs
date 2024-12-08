namespace RTG
{
    public class SphereColliderChangedAction : IUndoRedoAction
    {
        private SphereColliderSnapshot _preChangeSnapshot;
        private SphereColliderSnapshot _postChangeSnapshot;

        public SphereColliderChangedAction(SphereColliderSnapshot preChangeSnapshot, SphereColliderSnapshot postChangeSnapshot)
        {
            _preChangeSnapshot = new SphereColliderSnapshot(preChangeSnapshot);
            _postChangeSnapshot = new SphereColliderSnapshot(postChangeSnapshot);
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
