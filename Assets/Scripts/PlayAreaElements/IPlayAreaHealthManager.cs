namespace MatchThreePrototype.PlayAreaElements
{

    public interface IPlayAreaHealthManager
    {

        public void TakeDamage(float points);

        public void HealDamage(float points);

        public void HealCompletely();

    }
}
