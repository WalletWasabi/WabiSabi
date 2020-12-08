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

### Current Wasabi