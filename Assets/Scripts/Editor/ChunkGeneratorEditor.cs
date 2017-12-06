using UnityEditor;

[CustomEditor(typeof(ChunkGenerator))]
public class ChunkGeneratorEditor : Editor
{

    private void OnDestroy()
    {
        ChunkGenerator gen = target as ChunkGenerator;
        if (!gen)
        {
            gen.datas
                .DiscardEventReferences();
        }
    }

}
