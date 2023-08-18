using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlanetGenerator))]
public class PlanetEditor : Editor
{
    private PlanetGenerator planetGenerator;
    private Editor shapeEditor;
    private Editor colorEditor;

    public void OnEnable() {
        planetGenerator = (PlanetGenerator)target;
    }

    public override void OnInspectorGUI() {
        using (var checkScope = new EditorGUI.ChangeCheckScope()) {
            base.OnInspectorGUI();

            if (checkScope.changed) {
                planetGenerator.Generate();
            }
        }

        if (GUILayout.Button("Generate")) {
            planetGenerator.Generate();
        }

        DrawSettingsObject(
            planetGenerator.shapeSettings, 
            planetGenerator.OnShapeSettingsUpdated,
            ref planetGenerator.ShapeSettingsIsVisible(),
            ref shapeEditor
        );
        
        DrawSettingsObject(
            planetGenerator.colorSettings, 
            planetGenerator.OnColorSettingsUpdated,
            ref planetGenerator.ColorSettingsIsVisible(),
            ref colorEditor
        );
    }

    private void DrawSettingsObject(Object settings, System.Action onSettingsUpdated, ref bool isVisible, ref Editor editor) {
        if (settings == null) {
            return;
        }

        isVisible = EditorGUILayout.InspectorTitlebar(
            isVisible,
            settings
        );

        using (var checkScope = new EditorGUI.ChangeCheckScope()) {
            if (isVisible) {
                CreateCachedEditor(
                    settings, 
                    null, 
                    ref editor
                );
                
                editor.OnInspectorGUI();

                if (checkScope.changed && onSettingsUpdated != null) {
                    onSettingsUpdated();
                }
            }
        }
    }
}
