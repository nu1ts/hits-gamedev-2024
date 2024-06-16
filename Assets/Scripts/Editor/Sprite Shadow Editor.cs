using UnityEngine.UIElements;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(SpriteShadow))]
    public class SpriteShadowEditor : UnityEditor.Editor
    {
        public VisualTreeAsset visualTree;
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            visualTree.CloneTree(root);
            
            var targetScript = (SpriteShadow)target;
            
            var foldout = root.Q<Foldout>("VisibilityFoldout");
            
            for (var i = 0; i < targetScript.visibility.Length; i++)
            {
                var index = i;
                var toggle = new Toggle(targetScript.spriteNames[index])
                {
                    value = targetScript.visibility[index]
                };
                
                toggle.RegisterValueChangedCallback(evt =>
                {
                    targetScript.visibility[index] = evt.newValue;
                    EditorUtility.SetDirty(targetScript);
                });

                foldout.contentContainer.Add(toggle);
            }

            return root;
        }
    }
}