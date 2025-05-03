using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

public class GauntletVisuals : MonoBehaviour
{
    [SerializeField]
    private GameObject particle;
    private GameObject Particle => particle;
    [SerializeField]
    private float particleSpeed;
    private float ParticleSpeed => particleSpeed;
    [SerializeField]
    private float particleRotateSpeed;
    private float ParticleRotateSpeed => particleRotateSpeed;
    [SerializeField]
    private float particleRotateAcceleration;
    private float ParticleRotateAcceleration => particleRotateAcceleration;
    [SerializeField]
    private GameObject hitParticle;
    private GameObject HitParticle => hitParticle;

    private List<ParticleData> Particles { get; set; }
    private Transform Target { get; set; }
    private Action OnComplete { get; set; }

    private class ParticleData
    {
        public Transform Transform { get; set; }
        public Vector3 Target { get; set; }
        public float RotationSpeed { get; set; }
    }


    [EasyButtons.Button]
    public void PlayEffect(Transform target, Action onComplete = null)
    {
        OnComplete = onComplete;
        Target = target;
        Particles = new List<ParticleData>();
        InstantiateParticles().RunParallel();
        AudioController.Instance?.PlayLocalSound("GauntletBeam", target.gameObject);
    }

    private async Task InstantiateParticles()
    {
        int[] indexes = Enumerable.Range(0, 8).ToArray();
        Shuffle(indexes);
        for (int i = 0; i < 8; i++)
        {
            GameObject particle = Instantiate(Particle, transform.position, Quaternion.identity);

            Quaternion forwardRot = Quaternion.AngleAxis(240 + ((230 / 8) * indexes[i]), transform.forward);
            particle.transform.rotation = Quaternion.LookRotation(forwardRot * Vector3.up, forwardRot * Vector3.forward);
            Vector3 target = Target.position;
            Vector2 randomWithinDisc = UnityEngine.Random.insideUnitCircle * 0.4f;
            target += Target.forward * 0.1f;
            target += (Target.up * randomWithinDisc.y) + (Target.right * randomWithinDisc.x);

            ParticleData particleData = new ParticleData()
            {
                Transform = particle.transform,
                Target = target,
                RotationSpeed = ParticleRotateSpeed
            };
            Particles.Add(particleData);
            await Task.Delay(TimeSpan.FromSeconds(0.05));
        }
    }

    void Shuffle(int[] array)
    {
        int p = array.Length;
        for (int n = p - 1; n > 0; n--)
        {
            int r = UnityEngine.Random.Range(1, n);
            int t = array[r];
            array[r] = array[n];
            array[n] = t;
        }
    }

    [EasyButtons.Button]
    public void Cleanup()
    {
        foreach (var particle in Particles)
        {
            Destroy(particle.Transform.gameObject);
        }
        Particles = new List<ParticleData>();

    }


    void Update()
    {
        if (Particles == null)
        {
            return;
        }

        for (int i = 0; i < Particles.Count; i++)
        {
            ParticleData particleData = Particles[i];
            if (particleData.Transform == null)
            {
                Particles.Remove(particleData);
                continue;
            }

            particleData.RotationSpeed += Time.deltaTime * ParticleRotateAcceleration;

            Transform particleTransform = particleData.Transform;
            Vector3 targetDirection = particleData.Target - particleTransform.position;
            targetDirection = targetDirection.normalized;

            float step = particleData.RotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(particleTransform.forward, targetDirection, step, 0.0f);
            particleTransform.rotation = Quaternion.LookRotation(newDirection);
            particleTransform.position += particleTransform.forward * ParticleSpeed * Time.deltaTime;

            if (Vector3.Distance(particleData.Target, particleTransform.position) <= 0.05f)
            {
                GameObject hitParticle = Instantiate(HitParticle, particleTransform.position, Quaternion.identity);
                Destroy(hitParticle, 0.6f);
                Particles.Remove(particleData);
                //Destroy(particleTransform.gameObject);

            }
        }

        if (Particles.Count == 0)
        {
            OnComplete?.Invoke();
            OnComplete = null;
        }
    }
}
