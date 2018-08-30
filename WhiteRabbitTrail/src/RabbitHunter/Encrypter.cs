namespace RabbitHunterTests
{
    public interface Encrypter
    {
        string Hash(string phrase);
    }
}