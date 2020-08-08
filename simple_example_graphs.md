

# Notes

-   These example are meant to illustrate how the building blocks work and consequences/trade-offs of applying them in various ways, not all examples are desirable. Costs for privacy mostly reduce as the examples progress.
-   A single user's outputs are highlighted in various example transaction graphs.
-   Time flows up and arrows from inputs point at previous outputs in line with Satoshi's true original vision.
-   To keep illustrated CoinJoin transactions to a reasonable size most examples have 3 participants and arbitrary input and output amount registrations are always allowed (no coordinator enforced anonymity set). Previous mixes are always simplified to an equal amount CoinJoin with 3 participants.
-   Denominations are based on 1-2-5 preferred value series (powers of 2 omitted for simplicity, but both should be used in practice).
-   Mining and coordinator fees are always add to 0.0001. In practice the dust limit should parameterize any rounding, and total fees would be a function of the amounts and the total weight, not a constant.
-   Coordinator fee outputs are only shown (with black background) when accounting for prepaid fees, otherwise the coordinator's fee output is omitted.
-   The anonymity set size of the user's output is loosely represented by color (red, yellow, green). This does not account for inherited anonymity sets, it's intended to emphasize relative change in privacy of including a specific input or output in a given context.
-   An output's anonymity set can degrade when spent if the input can be linked to sibling inputs whose previous outputs have a smaller anonymity set.


# Single Operation

These example illustrate receiving and sweeping or remixing of a single
output. Standard denomination outputs, which are likely to have a larger
anonymity set, are created. Standard denominations may still need to be
subsequently remixed to ensure privacy since even if the coordinator enforces
\(k > 0\), that only applies to arbitrary denominations.


## Sweep and mix into standard denomination outputs

-   Sweep and mix amount close to standard denomination
    
    \( \{ 0.1001 \} \mapsto \{ 0.1 \} \)
    
    ![img](diagrams/txs/01.svg)

-   Sweep and mix standard denomination amount with auxiliary input for fees
    
    \( \{ 0.1, 0.0001 \} \mapsto \{ 0.1 \} \)
    
    ![img](diagrams/txs/02.svg)

-   Sweep and mix standard denomination amount, creating a mess of outputs
    
    \( \{ 0.1 \} \mapsto \{ 0.05, 0.02, 0.02, 0.005, 0.002, 0.002, 0.0005, 0.0002, 0.0002 \} \)
    
    ![img](diagrams/txs/03.svg)

-   Sweep and mix arbitrary amount, creating a mess of outputs
    
    \( \{ 0.736 \} \mapsto \{ 0.5, 0.2, 0.02, 0.01, 0.005, 0.0005, 0.0002, 0.0002 \} \)
    
    ![img](diagrams/txs/04.svg)

-   Sweep and mix arbitrary amount with auxiliary mixed outputs for rounding of created output
    
    \( \{ 0.736, 0.2, 0.05, 0.01, 0.002, 0.002, 0.0001 \} \mapsto \{ 1.0 \} \)
    
    ![img](diagrams/txs/05.svg)


## Reduce block space requirement and UTXO set churn using optional fee credentials

By adding support for [prepaid fee credentials](https://github.com/zkSNACKs/WabiSabi/issues/18) small change outputs can be
suppressed, instead issuing prepaid fee credentials that can then be redeemed
in subsequent rounds in order to pay the required mining and coordinator fees
(the coordinator may need to include additional inputs to cover mining fees
in the event that participants' non-prepaid coordination fees can't cover the
mining fees).

-   Sweep and mix standard denomination utilizing prepaid fee credential to cover mining and coordination fees
    
    \( \{ 0.1, \underbrace{0.0001}_\mathrm{prepaid} \} \mapsto \{ 0.1 \} \)
    
    ![img](diagrams/txs/07.svg)

-   Sweep and mix amount close to standard denomination, suppressing smaller outputs by prepaying fees
    
    \( \{ 0.1002 \} \mapsto \{ 0.1, \underbrace{0.0001}_\mathrm{prepaid} \} \)
    
    ![img](diagrams/txs/08.svg)

-   Sweep and mix arbitrary amount, suppressing smaller outputs by prepaying fees credentials
    
    \( \{ 0.736 \} \mapsto \{ 0.5, 0.2, 0.02, 0.01, 0.005, \underbrace{0.0009}_\mathrm{prepaid} \} \)
    
    ![img](diagrams/txs/09.svg)

-   Sweep and mix arbitrary amount with auxiliary mixed outputs in order to create rounded amount, suppressing smaller outputs by prepaying fees
    
    \( \{ 0.736, 0.2, 0.05, 0.01, 0.005 \} \mapsto \{ 1.0, \underbrace{0.0009}_\mathrm{prepaid} \} \)
    
    ![img](diagrams/txs/10.svg)

-   Sweep and mix arbitrary amount with auxiliary mixed outputs in order to create rounded amount, covering fees with prepaid credential
    
    \( \{ 0.736, 0.2, 0.05, 0.01, 0.002, 0.002, \underbrace{0.0001}_\mathrm{prepaid} \} \mapsto \{ 1.0 \} \)
    
    ![img](diagrams/txs/11.svg)


## Spend mixed standard denomination outputs to make arbitrary amount payments

-   Spend using mixed output close to arbitrary payment amount creating toxic change
    
    Note that the anonymity set of the input is not degraded, the payment can't
    be traced to a specific previous output, but the change and payment are
    linkable, and the payment itself is known to the receiver, but strictly
    speaking the anonymity set of this pair of outputs is still 3.
    
    \( \{ 0.01 \} \mapsto \{ 0.00934178, 0.00055822 \} \)
    
    ![img](diagrams/txs/12.svg)

-   Spend using mixed output close to payment amount creating standard denomination change and suppressing dust change by prepaying fees
    
    \( \{ 0.01 \} \mapsto \{ 0.00934178, 0.0005, \underbrace{0.00005822}_\mathrm{prepaid} \} \)
    
    ![img](diagrams/txs/13.svg)

-   Spend using mixed output larger than payment amount creating multiple standard denomination changes and suppressing dust by prepaying fees
    
    In this example the payment can't be linked to any specific sibling output
    or any specific funding input, so it is considered more private than
    before. Note however that the input only has one sibling input, so payment
    privacy relies more on the inherited anonymity set from the previous mix
    than the sibling inputs.
    
    \( \{ 0.1 \} \mapsto \{ 0.00934178, 0.05, 0.02, 0.02, \underbrace{0.00055822}_\mathrm{prepaid} \} \)
    
    ![img](diagrams/txs/14.svg)


# Batched Operations

Batching operations can cut through the need for intermediate standard
denomination outputs significantly saving on fees. However, to maintain
privacy this depends on other users to provide cover by registering their own
inputs or outputs of plausibly related amounts. To ensure that individual
outputs spent or created in a batch can't be linked the wallet may need to
fall back to performing operations separately, incurring higher fees,
depending on the coordinator's minimum anonymity set policy and/or the
announced denominations. The coordinator can enforce privacy by setting the
round parameter \(k\) to some minimum value, ensuring that arbitrary amount
registrations may only be made adjacent to standard denomination inputs and
outputs that provide cover.

-   Batched sweep and mix consolidating several arbitrary amount inputs
    
    Although the swept inputs are not deterministically linkable, but privacy
    is marginal.
    
    \( \{ 0.736, 0.321 \} \mapsto \{ 1.0, 0.05, 0.005, 0.001, 0.0005, 0.0002, 0.0002 \} \)
    
    ![img](diagrams/txs/15.svg)

-   Batched sweep and mix several arbitrary amounts suppressing smaller outputs
    
    \( \{ 0.736, 0.321 \} \mapsto \{ 1.0, 0.05, 0.005, 0.001, \underbrace{0.0009}_\mathrm{prepaid} \} \)
    
    ![img](diagrams/txs/16.svg)

-   Batched sweep and mix several arbitrary amounts and standard denominations to produce standard denomination
    
    \( \{ 0.736, 0.2471, 0.01, 0.005, 0.002 \} \mapsto \{ 1.0 \} \)
    
    ![img](diagrams/txs/17.svg)

-   Same as previous example, but with more participants reducing linkability of user's individual inputs
    
    \( \{ 0.736, 0.2471, 0.01, 0.005, 0.002 \} \mapsto \{ 1.0 \} \)
    
    ![img](diagrams/txs/18.svg)

-   Batched payments using mixed output larger than total payment amount creating multiple standard denomination changes and suppressing dust by prepaying fees
    
    \( \{ 0.1 \} \mapsto \{ 0.03916501, 0.02638449, 0.00934178, 0.02, 0.005, \underbrace{0.00000872}_{\mathrm{prepaid}} \} \)
    
    ![img](diagrams/txs/19.svg)

-   Combined sweeping and payments suppressing small outputs with prepaid fee credentials
    
    \( \{ 0.736, 0.321, 0.2471, 0.02, 0.001 \} \mapsto \{ 0.03916501, 0.02638449, 0.00934178, 1.0, 0.2, 0.05, \underbrace{0.00010872}_\mathrm{prepaid} \} \)
    
    ![img](diagrams/txs/20.svg)

-   Same as previous example but with more participants
    
    \( \{ 0.736, 0.321, 0.2471, 0.02, 0.001 \} \mapsto \{ 0.03916501, 0.02638449, 0.00934178, 1.0, 0.2, 0.05, \underbrace{0.00010872}_\mathrm{prepaid} \} \)
    
    ![img](diagrams/txs/21.svg)

