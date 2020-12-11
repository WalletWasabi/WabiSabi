# Amount Organization Specification for Wasabi Wallet 2.0

## Target User

Peter McCormack

## Objectives

- Avoid homogenic coin distribution.
- Avoid UTXO bloat.
- Significantly lower minimum denominations compared to [WW1.](https://github.com/nopara73/ZeroLink/)
- Thrive for not generating non-mixed changes.
- Thrive for faster mixing.
- Thrive for more blockspace efficient mixing.

## Approach

Since this amount organization will be used in Wasabi Wallet 2.0, a sensible strategy is to start out with the current amount organization of WW1 and step by step improve upon that.

### Setup

In order to verify our reasoning and intuitions we [sampled 8000 input amounts](./AmountOrganization/AmountOrganization/Sample.txt) from Wasabi Wallet 1.0 coinjoins around December 2020, those have not been mixed before.  
We'll use in various ways throughout this document, mostly from code, which source can be found under [AmountOrganization solution.](./AmountOrganization)

#### BASE: WWI Skeleton

To start out, we took 50 inputs randomly out of our sample and created 40 random groupings - users - out of it. Then implemented WW1's mixing technique and got the following example output.

How does WW1's mixing technique work? It takes the amounts and loops them through powers of 2 denominations in ascending order. It creates outputs for every denomination level the user can still participate in. If it can't it creates a change output.

We shall also note that this example dismisses many variables, most importantly the minimum denomination Wasabi imposes and the network fees. We'll introduce them later on.

```
Number of users:        40
Number of inputs:       50
Number of outputs:      937

There are 40 occurrences of 0.00000001 BTC output.
There are 40 occurrences of 0.00000002 BTC output.
There are 40 occurrences of 0.00000004 BTC output.
There are 40 occurrences of 0.00000008 BTC output.
There are 40 occurrences of 0.00000016 BTC output.
There are 40 occurrences of 0.00000032 BTC output.
There are 40 occurrences of 0.00000064 BTC output.
There are 40 occurrences of 0.00000128 BTC output.
There are 40 occurrences of 0.00000256 BTC output.
There are 40 occurrences of 0.00000512 BTC output.
There are 40 occurrences of 0.00001024 BTC output.
There are 40 occurrences of 0.00002048 BTC output.
There are 40 occurrences of 0.00004096 BTC output.
There are 40 occurrences of 0.00008192 BTC output.
There are 40 occurrences of 0.00016384 BTC output.
There are 40 occurrences of 0.00032768 BTC output.
There are 40 occurrences of 0.00065536 BTC output.
There are 1 occurrences of 0.00091212 BTC output.
There are 40 occurrences of 0.00131072 BTC output.
There are 1 occurrences of 0.00133484 BTC output.
There are 1 occurrences of 0.00139596 BTC output.
There are 1 occurrences of 0.00160831 BTC output.
There are 1 occurrences of 0.00204713 BTC output.
There are 1 occurrences of 0.00232849 BTC output.
There are 1 occurrences of 0.00250764 BTC output.
There are 1 occurrences of 0.00251425 BTC output.
There are 40 occurrences of 0.00262144 BTC output.
There are 1 occurrences of 0.00319055 BTC output.
There are 1 occurrences of 0.00501425 BTC output.
There are 34 occurrences of 0.00524288 BTC output.
There are 1 occurrences of 0.00628344 BTC output.
There are 1 occurrences of 0.00766987 BTC output.
There are 1 occurrences of 0.008003 BTC output.
There are 1 occurrences of 0.00889539 BTC output.
There are 31 occurrences of 0.01048576 BTC output.
There are 1 occurrences of 0.01064874 BTC output.
There are 1 occurrences of 0.01079982 BTC output.
There are 1 occurrences of 0.01080556 BTC output.
There are 1 occurrences of 0.01237489 BTC output.
There are 1 occurrences of 0.01273657 BTC output.
There are 1 occurrences of 0.01345343 BTC output.
There are 1 occurrences of 0.0138582 BTC output.
There are 1 occurrences of 0.01415412 BTC output.
There are 1 occurrences of 0.0147032 BTC output.
There are 1 occurrences of 0.01599117 BTC output.
There are 1 occurrences of 0.01711393 BTC output.
There are 1 occurrences of 0.01849849 BTC output.
There are 28 occurrences of 0.02097152 BTC output.
There are 1 occurrences of 0.02611393 BTC output.
There are 1 occurrences of 0.03132254 BTC output.
There are 1 occurrences of 0.031777 BTC output.
There are 1 occurrences of 0.0360725 BTC output.
There are 1 occurrences of 0.0384578 BTC output.
There are 19 occurrences of 0.04194304 BTC output.
There are 1 occurrences of 0.05586343 BTC output.
There are 1 occurrences of 0.06800123 BTC output.
There are 1 occurrences of 0.06908003 BTC output.
There are 12 occurrences of 0.08388608 BTC output.
There are 1 occurrences of 0.10004366 BTC output.
There are 1 occurrences of 0.10557624 BTC output.
There are 1 occurrences of 0.10746535 BTC output.
There are 1 occurrences of 0.11578316 BTC output.
There are 1 occurrences of 0.13324562 BTC output.
There are 1 occurrences of 0.1543588 BTC output.
There are 7 occurrences of 0.16777216 BTC output.
There are 4 occurrences of 0.33554432 BTC output.
There are 2 occurrences of 0.67108864 BTC output.
```

#### Dust Strategy

Our next thought was to make a drastic change on the amount organization structure, but we realized the nature of the change may depend on how fees are handled and that may depend on our dust handling strategy, thus it's reasonable to continue with that.  
So far we've created a transaction that isn't standard, because many outputs, like one satoshi outputs are present. To manage this issue and make our transaction standard, we want to make sure users don't register small outputs by setting a dust limit.  
Dismissing the dust by the user is fine, because it's not significant amount. Let's assume our dust limit is 1000 satoshis, which should satisfy the standardness rules.

```
Number of users:        40
Number of inputs:       50
Number of outputs:      538

There are 40 occurrences of 0.00001024 BTC output.
There are 40 occurrences of 0.00002048 BTC output.
There are 40 occurrences of 0.00004096 BTC output.
There are 1 occurrences of 0.00007692 BTC output.
There are 40 occurrences of 0.00008192 BTC output.
There are 40 occurrences of 0.00016384 BTC output.
There are 39 occurrences of 0.00032768 BTC output.
There are 39 occurrences of 0.00065536 BTC output.
There are 1 occurrences of 0.00084648 BTC output.
There are 39 occurrences of 0.00131072 BTC output.
There are 1 occurrences of 0.00138943 BTC output.
There are 1 occurrences of 0.00140782 BTC output.
There are 1 occurrences of 0.00211015 BTC output.
There are 39 occurrences of 0.00262144 BTC output.
There are 1 occurrences of 0.00281426 BTC output.
There are 1 occurrences of 0.00399417 BTC output.
There are 36 occurrences of 0.00524288 BTC output.
There are 1 occurrences of 0.00550693 BTC output.
There are 1 occurrences of 0.00567492 BTC output.
There are 1 occurrences of 0.00710041 BTC output.
There are 1 occurrences of 0.00836727 BTC output.
There are 1 occurrences of 0.00953079 BTC output.
There are 1 occurrences of 0.01030543 BTC output.
There are 1 occurrences of 0.01034971 BTC output.
There are 35 occurrences of 0.01048576 BTC output.
There are 1 occurrences of 0.0105057 BTC output.
There are 1 occurrences of 0.01222353 BTC output.
There are 1 occurrences of 0.01252416 BTC output.
There are 1 occurrences of 0.01273416 BTC output.
There are 1 occurrences of 0.01432513 BTC output.
There are 1 occurrences of 0.01467416 BTC output.
There are 1 occurrences of 0.01493107 BTC output.
There are 1 occurrences of 0.01509005 BTC output.
There are 1 occurrences of 0.01551344 BTC output.
There are 1 occurrences of 0.01655845 BTC output.
There are 1 occurrences of 0.01681699 BTC output.
There are 26 occurrences of 0.02097152 BTC output.
There are 1 occurrences of 0.02388936 BTC output.
There are 1 occurrences of 0.02922127 BTC output.
There are 1 occurrences of 0.03322046 BTC output.
There are 1 occurrences of 0.03623939 BTC output.
There are 1 occurrences of 0.03748776 BTC output.
There are 15 occurrences of 0.04194304 BTC output.
There are 1 occurrences of 0.04467416 BTC output.
There are 9 occurrences of 0.08388608 BTC output.
There are 1 occurrences of 0.10879512 BTC output.
There are 1 occurrences of 0.11102965 BTC output.
There are 9 occurrences of 0.16777216 BTC output.
There are 1 occurrences of 0.2740125 BTC output.
There are 1 occurrences of 0.31065568 BTC output.
There are 2 occurrences of 0.3289216 BTC output.
There are 8 occurrences of 0.33554432 BTC output.
There are 1 occurrences of 0.54129616 BTC output.
There are 3 occurrences of 0.67108864 BTC output.
There are 1 occurrences of 0.757806 BTC output.
There are 1 occurrences of 0.98800148 BTC output.
There are 1 occurrences of 1.34217728 BTC output.
```

Although we handled our standardness problem, we also want to make sure we don't create uneconomical outputs. Our working formula for this is: `MAX((feerate * input spend size estimation), 1000, (minimum relay fee rate * input spend size estimation))`.
If we choose the current fee rate to 10s/vb, and estimate our input spend size to 69 bytes, then we'll still be under the 1000 satoshi sanity fee rate, so we don't need to illustrate our new results.

#### Fee Strategy

In order to account for real world mining fees we subtracted the estimated amounts paid for every input and every output.

```
Number of users:        40
Number of inputs:       50
Number of outputs:      517
Total in:               37.00194392 BTC
Fee paid:               0.00205110 BTC
Size:                   20511 vbyte
Fee rate:               10 sats/vbyte

There are 40 occurrences of 0.00001024 BTC output.
There are 40 occurrences of 0.00002048 BTC output.
There are 40 occurrences of 0.00004096 BTC output.
There are 1 occurrences of 0.00004171 BTC output.
There are 39 occurrences of 0.00008192 BTC output.
There are 39 occurrences of 0.00016384 BTC output.
There are 39 occurrences of 0.00032768 BTC output.
There are 1 occurrences of 0.00055742 BTC output.
There are 1 occurrences of 0.00064285 BTC output.
There are 39 occurrences of 0.00065536 BTC output.
There are 1 occurrences of 0.00110742 BTC output.
There are 39 occurrences of 0.00131072 BTC output.
There are 1 occurrences of 0.00134146 BTC output.
There are 1 occurrences of 0.00141019 BTC output.
There are 1 occurrences of 0.00181787 BTC output.
There are 1 occurrences of 0.00230577 BTC output.
There are 1 occurrences of 0.00235743 BTC output.
There are 39 occurrences of 0.00262144 BTC output.
There are 1 occurrences of 0.0047017 BTC output.
There are 35 occurrences of 0.00524288 BTC output.
There are 1 occurrences of 0.00580139 BTC output.
There are 1 occurrences of 0.0062007 BTC output.
There are 1 occurrences of 0.00708982 BTC output.
There are 1 occurrences of 0.00719311 BTC output.
There are 1 occurrences of 0.00741736 BTC output.
There are 1 occurrences of 0.00789242 BTC output.
There are 1 occurrences of 0.00799932 BTC output.
There are 1 occurrences of 0.00813015 BTC output.
There are 1 occurrences of 0.00869702 BTC output.
There are 1 occurrences of 0.00996575 BTC output.
There are 1 occurrences of 0.01027362 BTC output.
There are 26 occurrences of 0.01048576 BTC output.
There are 1 occurrences of 0.01318739 BTC output.
There are 1 occurrences of 0.01339224 BTC output.
There are 1 occurrences of 0.01445627 BTC output.
There are 1 occurrences of 0.01462106 BTC output.
There are 1 occurrences of 0.01607106 BTC output.
There are 1 occurrences of 0.01707106 BTC output.
There are 1 occurrences of 0.01757983 BTC output.
There are 1 occurrences of 0.01762349 BTC output.
There are 1 occurrences of 0.01849946 BTC output.
There are 1 occurrences of 0.02002486 BTC output.
There are 23 occurrences of 0.02097152 BTC output.
There are 1 occurrences of 0.03033266 BTC output.
There are 1 occurrences of 0.0393451 BTC output.
There are 14 occurrences of 0.04194304 BTC output.
There are 1 occurrences of 0.05653016 BTC output.
There are 1 occurrences of 0.07940192 BTC output.
There are 7 occurrences of 0.08388608 BTC output.
There are 1 occurrences of 0.11869243 BTC output.
There are 1 occurrences of 0.14460622 BTC output.
There are 5 occurrences of 0.16777216 BTC output.
There are 1 occurrences of 0.2086305 BTC output.
There are 4 occurrences of 0.33554432 BTC output.
There are 1 occurrences of 0.65823 BTC output.
There are 3 occurrences of 0.67108864 BTC output.
There are 2 occurrences of 1.34217728 BTC output.
There are 2 occurrences of 2.68435456 BTC output.
There are 1 occurrences of 4.72813231 BTC output.
There are 1 occurrences of 5.36870912 BTC output.
There are 1 occurrences of 10.73741824 BTC output.
```

#### Descending Powers of 2

It would be more efficient to do descending powers of 2 mixing instead of ascending powers of 2 mixing like we did before. So we implemented this and here's what we noticed:

- Now more dust coins would be created, but because of their low economic value, these coins are not created and instead the value is added to the miner fees. Note that dust warnings are ommitted on the output pasted here.
- We're creating roughly half the number of outputs, which is gret for blockspace.  
- We've nearly completely eliminated change.  

Furthermore a few more small optimization was introduced:

- Our sanity dust threshold became 1024, so we won't create smaller change than that.  
- A user now also consider the largest output that isn't her so she won't create denomination that would be larger than the largest possible user's output she could reasonably mix with.
- Finally we considered breaking down the largest user's change output, to the largest possible denomination, but we couldn't decide, so that'll be a later step.

Our simulation also got an improvement as now we're also simulating a 30% remix count by doing a pre-mix, then select 30% of those inputs to create our random groups and have results from that.

```
Number of users:        40
Number of inputs:       50
Number of outputs:      279
Total in:               13.16067121 BTC
Fee paid:               0.00147011 BTC
Size:                   12657 vbyte
Fee rate:               11 sats/vbyte

There are 17 occurrences of     0.00001024 BTC output.
There are 15 occurrences of     0.00002048 BTC output.
There are 22 occurrences of     0.00004096 BTC output.
There are 24 occurrences of     0.00008192 BTC output.
There are 22 occurrences of     0.00016384 BTC output.
There are 23 occurrences of     0.00032768 BTC output.
There are 22 occurrences of     0.00065536 BTC output.
There are 20 occurrences of     0.00131072 BTC output.
There are 21 occurrences of     0.00262144 BTC output.
There are 20 occurrences of     0.00524288 BTC output.
There are 16 occurrences of     0.01048576 BTC output.
There are 12 occurrences of     0.02097152 BTC output.
There are 15 occurrences of     0.04194304 BTC output.
There are 11 occurrences of     0.08388608 BTC output.
There are 6 occurrences of      0.16777216 BTC output.
There are 6 occurrences of      0.33554432 BTC output.
There are 3 occurrences of      0.67108864 BTC output.
There are 3 occurrences of      1.34217728 BTC output.
There are 1 occurrences of      1.91315182 BTC output.
```

#### Should we break down the largest user's bag?
A user whose combined input amount is significantly larger (or smaller) than the other users always face difficulties in mixing. 
In this subsection we address different approaches to managing large output amounts. Smaller amounts are not addressed here.
We identified five possible approaches. 

The trivial approach is not to break the large input(s) up. 
We just produce a large amount change minus the fees. This output is deterministically linkable to its input cluster.
Potentially, it might also reveal what other amounts were used by the user.

Next, we consider different ways of splitting up the large amounts. 
If these amounts are split up too granularly, then a single user with more liquidity than others is indistinguishable from a sybil attack.
Additionally, this user gains little privacy themselves (probabilistic privacy instead of k-anonymity). 
Moreover, this approach is not blockspace efficient.
On the other hand, if they are split up too coarsely, then that user's amounts are deterministically linkable to their inputs.

We discuss the following approaches for splitting up the large amount inputs:
The following design decisions need to be made.
  
Amount selection (How does the wallet select the amounts subtracted from the large input?) 
 - Standard denominations   
 - Knapsack
 
Limits of the process:
 - One-shot
 - iterative
   - deterministic
   - probabilistic
   
  
Coinflip: we could combine the two approaches probabilistically by flipping a random coin. 


