# Collaborative Transactions

Generalization of Chaumian CoinJoins. Allows sending arbitrary amounts in CoinJoins and avoids creating unmixed change.

## 1. Input Registration

Peer registers an input along with blinded blinded random certificates and ask the coordinator to sign the blind certificates. The certificates must follow 1-2-5 series. 

### Examples

For example if peer registers 1BTC, then he can ask for a signature for a 1BTC certificate or can also ask for 5 signatures for 0.1BTC certificates and 1 signature for a 0.5BTC certificate.  

### Notes

Since the coordinator must communicate numerous public keys to peers, the communication overhead is large. To solve this key generators have to be utilized.

Peers can register any number of inputs, but through new anonymity network identities to avoid input linkages by the coordinator.

## 2. Payment Intent Registration

Peer registers arbitrary payment intents with his certificates.

### Examples

Peer got a 1BTC certificate and would like to send 0.1234 BTC to an address, then his payment intend would be 0.1234BTC - address and 0.8766BTC - address2.  

### 3. Signing + Blame

Transaction goes out for signing. The input that does not sign the transaction will be prohibited from further participation.

## Randomization

Randomization (along desired distributions) key for number of inputs registered, certificate size requests and payment intent registrations are important in order to make sure heuristics cannot be applied to them.
